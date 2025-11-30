using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// 인벤토리 UI 윈도우를 제어하는 클래스입니다.
/// 아이템, 능력치, 장비 정보를 표시하고, 창 열기/닫기 및 관련 이벤트를 처리합니다.
/// </summary>
public class InventoryWindow : MonoBehaviour, IUIWindow
{
    //이 클래스에 필요한 인터페이스들 (의존성 주입 대상)
    private IItemManager itemManager;
    private IPlayerAbilityManager playerAbilityManager;
    private IEquipmentManager equipmentManager;

    [SerializeField] private UnityEvent onOpen; //창 열릴 때 발생하는 이벤트
    [SerializeField] private UnityEvent onClose;    //창 닫힐 때 발생하는 이벤트

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;   //창 열림/닫힘 보고 이벤트

    //인벤토리 윈도우의 클래스 요소들
    [SerializeField] private InventoryItemPanel inventoryItemPanel; //아이템 패널
    [SerializeField] private InventoryAbilityPanel inventoryAbilityPanel;   //능력치 패널
    [SerializeField] private ItemInfoPanelCreator itemInfoPanelCreator; //아이템 정보 패널 생성기
    [SerializeField] private InventoryEquipmentPanel equipmentPanel;    //장비 패널

    [SerializeField] private Button exitButton; //닫기 버튼
    [SerializeField] private TextMeshProUGUI goldAmountTmp; //골드 표시 텍스트

    private bool isOpenFirst = true;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    /// <summary>
    /// 외부에서 의존성 주입을 받아 인터페이스를 설정합니다.
    /// </summary>
    /// <param name="_dependencies"></param>
    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IItemManager item)
                itemManager = item;
            if (dependency is IPlayerAbilityManager pam)
                playerAbilityManager = pam;
            if (dependency is IEquipmentManager iem)
                equipmentManager = iem;
        }
    }

    /// <summary>
    /// 초기 데이터를 받아 설정합니다.
    /// </summary>
    /// <param name="_datas"></param>
    public void Initial(object[] _datas)
    {
        //각 패널 초기화
        inventoryItemPanel.Initial(itemManager, equipmentManager);
        inventoryAbilityPanel.Initial(playerAbilityManager);
        equipmentPanel.Initial(equipmentManager, itemManager);

        //골드 표시 및 이벤트 연결
        if (itemManager != null)
        {
            goldAmountTmp.text = itemManager.GoldAmount.ToString();
            itemManager.OnGoldAmountChanged += OnGoldAmountChanged;
        }
    }

    /// <summary>
    /// 인벤토리 창을 엽니다.
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //UI를 최상단으로 배치

        if(openEvent != null)
            openEvent.RaiseOpenWindow(this);    //창 열림 이벤트 보고

        if (isOpenFirst)
        {

        }

        isOpenFirst = false;
        onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        //골드 변경 이벤트 해제
        if (itemManager != null)
        {
            itemManager.OnGoldAmountChanged -= OnGoldAmountChanged;
        }

        //각 패널 닫기 처리
        if (equipmentPanel != null)
            equipmentPanel.OnCloseInvenPanel();
        if (inventoryItemPanel != null)
            inventoryItemPanel.OnCloseInven();
        if (inventoryAbilityPanel != null)
            inventoryAbilityPanel.OnCloseInven();

        itemInfoPanelCreator.closeAllPanel();   //모든 정보 패널 닫기
        onClose?.Invoke();
    }

    /// <summary>
    /// 골드 변경 시 UI 텍스트 갱신
    /// </summary>
    /// <param name="_amount"></param>
    private void OnGoldAmountChanged(int _amount)
    { 
        goldAmountTmp.text = _amount.ToString();
    }

    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }

    /// <summary>
    /// 오브젝트 파괴 시 이벤트 해제
    /// </summary>
    private void OnDestroy()
    {
        if (itemManager != null)
        {
            itemManager.OnGoldAmountChanged -= OnGoldAmountChanged;
        }
    }
}
