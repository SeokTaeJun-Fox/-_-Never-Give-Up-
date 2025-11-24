using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

//장비 아이템 정보 패널을 제어하는 UI 컨트롤러입니다.
//아이템의 능력치, 강화 상태, 착용 여부 등을 시각적으로 표시하며,
//착용/해제 버튼을 통해 장비 시스템과 상호작용합니다.
public class EquipmentItemInfoPanelController : MonoBehaviour, IUIWindow
{
    [SerializeField] private UnityEvent onClose; //패널이 닫힐 때 호출되는 이벤트

    [SerializeField] private EquipmentItemInfoPanelView view; //실제 UI 요소를 제어하는 뷰 클래스
    private ItemStack itemStack;    //보여줄 아이템스택(아이템 + 수량) 정보
    private EquipmentItem item;     //보여줄 장비 아이템

    //이 클래스에 필요한 인터페이스들
    private IEquipmentManager equipmentManager;
    private IItemManager itemManager;

    //이 클래스에 필요한 이벤트스크립트오브젝트들

    //패널 열림/닫힘 이벤트를 외부에 알리는 스크립터블 오브젝트 (이벤트 채널)
    [SerializeField] private ReportOpenWindowEvent openEvent;

    //기타
    [SerializeField] private Color upgradeStatColor; //능력치 상승 시 표시할 색상
    [SerializeField] private Color downgradeStatColor; //능력치 하락 시 표시할 색상
    [SerializeField] private Color normalStatColor; //능력치 변화가 없을 때 표시할 색상
    [SerializeField] private Color enchantedItemStringColor; //강화된 아이템 이름에 사용할 색상
    [SerializeField] private Color noEnchantedItemStringColor;  //강화되지 않은 아이템 이름에 사용할 색상

    private void Awake()
    {
        view.CloseButton.onClick.AddListener(OnClickCloseButton);
        view.EquipButton.onClick.AddListener(OnClickEquipButton);
        view.UnEquipButton.onClick.AddListener(OnClickUnEquipButton);
    }

    //인터페이스 의존성 주입 (장비 매니저, 아이템 매니저)
    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IEquipmentManager em)
                equipmentManager = em;
            if (dependency is IItemManager im)
                itemManager = im;
        }
    }

    //패널 초기화
    //전달받은 아이템 데이터 기반으로 UI 세팅
    public void Initial(object[] _datas)
    {
        foreach (var data in _datas)
        {
            if (data is ItemStack stack)
            {
                itemStack = stack;
                item = itemStack.item as EquipmentItem;
                Setting();
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);
    }

    public void Close()
    {
        view.Initial();
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //아이템 정보를 기반으로 UI를 구성합니다.
    //이미지, 이름, 강화 상태, 능력치 비교, 버튼 상태 등을 처리합니다.
    private void Setting()
    {
        if (equipmentManager == null || itemManager == null)
            return;
        if (itemStack == null || item == null)
            return;

        EquipmentItem curEquippedItem = equipmentManager.GetEquipmentItem(item.EquipmentType);

        //아이템 이미지 보여주기
        view.ShowItemImage(item.Icon);

        //아이템 착용표시 보여주기
        if (curEquippedItem == null)
        {
            view.ShowEquippedSignUi(false);
        }
        else
        {
            view.ShowEquippedSignUi(curEquippedItem.ItemKey == item.ItemKey);
        }

        //아이템 이름,카테고리 보여주기
        string itemName = item.ItemName;
        if (item.CurEnchantedCount != 0)
        {
            itemName += $" (+{item.CurEnchantedCount})";
            view.ShowItemTitle(itemName, item.EquipmentType, enchantedItemStringColor);
        }
        else
        {
            view.ShowItemTitle(itemName, item.EquipmentType, noEnchantedItemStringColor);
        }

        //아이템 업그레이드 가능횟수 보여주기
        view.ShowUpgradeAmount(item.EnchantPossibleAmount);

        //아이템 스택 보여주기 로직
        Dictionary<string, int> equippedItemStatDic = equipmentManager.GetEquipmentItem(item.EquipmentType)?.GetAbilityDictionary();
        Dictionary<string, int> selectedItemStatDic = item.GetAbilityDictionary();

        //선택된 장비의 스텟 카테고리, 장착된 장비의 스텟 카테고리를 리스트에 저장합니다.
        List<string> equippedCategoryList = equippedItemStatDic?.Keys?.ToList();
        List<string> selectedCategoryList = selectedItemStatDic?.Keys?.ToList();

        List<string> mergedCategoryList;
        //두개의 카테고리 리스트를 겹치지 않게끔 머지시킵니다.
        if (equippedCategoryList == null)
        {
            mergedCategoryList = selectedCategoryList;
        }
        else
        { 
            mergedCategoryList = equippedCategoryList.Union(selectedCategoryList).ToList();
        }

        List<ItemStatViewData> statDatas = new();

        //장착된 장비가 없을때
        //선택된 장비 아이템의 스텟 그대로 적습니다.
        if (equippedItemStatDic == null)
        {
            foreach (var data in selectedCategoryList)
            { 
                ItemStatViewData itemStatViewData = new ItemStatViewData();
                itemStatViewData.statCategory = data;
                itemStatViewData.content = $"{selectedItemStatDic[data]} (+{selectedItemStatDic[data]})";
                itemStatViewData.color = upgradeStatColor;

                statDatas.Add(itemStatViewData);
            }
        }
        //장착된 장비가 있을때
        else
        {
            //스텟 카테고리, 스텟을 한줄 한줄 적습니다.
            foreach (var statCategory in mergedCategoryList)
            {
                //CASE1
                //장착된 장비와 선택된 장비의 스텟카테고리값이 존재하고,
                //장착된 장비와 선택된 장비의 스텟카테고리값이 같을때
                //선택된 장비의 스텟카테고리, 스텟카테고리값을 그대로 적습니다.
                if (selectedItemStatDic.ContainsKey(statCategory) && 
                    equippedItemStatDic.ContainsKey(statCategory) &&
                    equippedItemStatDic[statCategory] == selectedItemStatDic[statCategory])
                {
                    ItemStatViewData itemStatViewData = new ItemStatViewData();
                    itemStatViewData.statCategory = statCategory;
                    itemStatViewData.content = $"{selectedItemStatDic[statCategory]}";
                    itemStatViewData.color = normalStatColor;

                    statDatas.Add(itemStatViewData);
                }

                //CASE2
                //장착된 장비아이템의 스텟카테고리값은 존재하지만
                //선택된 장비아이템의 스텟카테고리값은 존재하지 않을경우
                //0을쓰고 괄호에 장착된 장비아이템의 스텟카테고리값을 음수로 적습니다.
                if (equippedItemStatDic.ContainsKey(statCategory) &&
                    !selectedItemStatDic.ContainsKey(statCategory))
                {
                    //스텟 차이를 변수에 저장합니다.
                    int statDifference = equippedItemStatDic[statCategory];

                    ItemStatViewData itemStatViewData = new ItemStatViewData();
                    itemStatViewData.statCategory = statCategory;
                    itemStatViewData.content = $"0 (-{statDifference})"; 
                    itemStatViewData.color = downgradeStatColor;

                    statDatas.Add(itemStatViewData);
                }

                //CASE3
                //장착된 장비아이템과 선택된 장비아이템의 스텟카테고리값은 존재하고
                //그 값이 장착된 장비아이템의값이 클경우
                //선택된 장비아이템의 값을 쓰고 괄호에 값의 차이를 음수로 적습니다.
                if (equippedItemStatDic.ContainsKey(statCategory) &&
                    selectedItemStatDic.ContainsKey(statCategory) &&
                    selectedItemStatDic[statCategory] < equippedItemStatDic[statCategory])
                {
                    //스텟 차이를 변수에 저장합니다.
                    int statDifference = equippedItemStatDic[statCategory] - selectedItemStatDic[statCategory];

                    ItemStatViewData itemStatViewData = new ItemStatViewData();
                    itemStatViewData.statCategory = statCategory;
                    itemStatViewData.content = $"{selectedItemStatDic[statCategory]} (-{statDifference})";
                    itemStatViewData.color = downgradeStatColor;

                    statDatas.Add(itemStatViewData);
                }

                //CASE4
                //장착된 장비아이템의 카테고리값이 존재하지 않거나,
                //선택된 장비아이템의 카테고리값이 더 클경우
                //선택된 장비아이템의 값을 쓰고 괄호에 선택된 장비아이템 값 혹은 그 차이를 양수로 적습니다.
                if (!equippedItemStatDic.ContainsKey(statCategory) ||
                    (selectedItemStatDic.ContainsKey(statCategory) && selectedItemStatDic[statCategory] > equippedItemStatDic[statCategory]))
                {
                    int statDifference = selectedItemStatDic[statCategory] - 
                        (!equippedItemStatDic.ContainsKey(statCategory) ? 0 : equippedItemStatDic[statCategory]);

                    ItemStatViewData itemStatViewData = new ItemStatViewData();
                    itemStatViewData.statCategory = statCategory;
                    itemStatViewData.content = $"{selectedItemStatDic[statCategory]} (+{statDifference})";
                    itemStatViewData.color = upgradeStatColor;

                    statDatas.Add(itemStatViewData);
                }
            }
        }

        view.ShowItemStat(statDatas);

        //하단 버튼 갱신
        if (itemManager.GetItemStack(item) == null)
        {
            view.ShowBottomButtons(EquipmentState.NOPOSSESSION);
        }
        else if (curEquippedItem == null || curEquippedItem.ItemKey != item.ItemKey)
        {
            view.ShowBottomButtons(EquipmentState.EQUIPPED);
        }
        else if (item.EquipmentType == EquipmentType.WEAPON && curEquippedItem.ItemKey == item.ItemKey)
        {
            view.ShowBottomButtons(EquipmentState.SAME_WEAPON_SELECTED);
        }
        else
        {
            view.ShowBottomButtons(EquipmentState.UNEQUIPPED);
        }
    }

    //이벤트
    private void OnClickCloseButton()
    {
        if (openEvent != null)
            openEvent.RaiseCloseWindow();

        Close();
    }

    //착용 버튼 클릭 시 호출됩니다.
    //선택된 아이템을 장착하고 패널을 닫습니다.
    private void OnClickEquipButton()
    {
        if (equipmentManager != null && item != null)
        {
            equipmentManager.Equip(item.EquipmentType, item);
        }

        if (openEvent != null)
            openEvent.RaiseCloseWindow();

        Close();
    }

    //해제 버튼 클릭 시 호출됩니다.
    //해당 부위의 장비를 해제하고 패널을 닫습니다.
    private void OnClickUnEquipButton()
    {
        if (equipmentManager != null && item != null)
        {
            equipmentManager.Equip(item.EquipmentType, null);
        }

        if (openEvent != null)
            openEvent.RaiseCloseWindow();

        Close();
    }
}
