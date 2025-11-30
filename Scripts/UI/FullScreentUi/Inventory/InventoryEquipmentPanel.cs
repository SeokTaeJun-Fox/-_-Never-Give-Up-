using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//인벤토리 UI에서 장비 슬롯을 제어하는 클래스입니다.
//장비 장착, 강화, 정보 패널 열기 등의 기능을 처리하며, 드롭 이벤트와 UI 뷰를 연결합니다.
public class InventoryEquipmentPanel : MonoBehaviour
{
    [SerializeField] private InventoryEquipmentPanelView view;  // 장비 UI 뷰
    [SerializeField] private List<EquipmentItemDropReceiver> dropReceivers; //드롭 수신기 리스트
    [SerializeField] private ItemInfoPanelCreator creator;  //아이템 정보 패널 생성기

    //이 클래스에 필요한 인터페이스
    private IEquipmentManager equipmentManager;
    private IItemManager itemManager;

    private void Awake()
    {
        //드롭 이벤트 연결
        foreach (var dropReceiver in dropReceivers)
        {
            dropReceiver.OnDropSuccessedEquipmentItem += OnDropSuccessedEquipmentItem;
            dropReceiver.OnDropSuccessedEnchantItem += OnDropSuccessedEnchantItem;
        }

        //버튼 클릭 시 정보 패널 열기 연결
        foreach (var infos in view.EquipmentAndPanelInfos)
        {
            EquipmentType type = infos.type;
            infos.button.onClick.AddListener(() =>
            {
                OpenItemInfoPanel(type);
            });
        }
    }

    //인벤토리 창을 열 때 초기화합니다.
    public void Initial(IEquipmentManager _equipmentManager, IItemManager _itemManager)
    {
        equipmentManager = _equipmentManager;
        itemManager = _itemManager;

        EquipAll();

        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged += OnEquip;
        }
    }

    //특정 장비 타입의 장비를 UI에 표시합니다.
    public void Equip(EquipmentType _type, EquipmentItem _item)
    {
        if (_item == null)
            view.Show(_type, null);
        else
            view.Show(_type, _item.Icon);

        EquipmentItemDropReceiver receiver = null;
        receiver = dropReceivers.Find(x => x.thisEquipmentType == _type);
        if (receiver != null)
        { 
            receiver.thisItem = _item;
        }
    }

    //모든 장비 슬롯을 UI에 표시합니다.
    private void EquipAll()
    {
        if (equipmentManager == null)
            return;

        IDictionary<EquipmentType, EquipmentItem> info = equipmentManager.EquippedItemDic;

        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        { 
            if(info.ContainsKey(type))
                Equip(type, info[type]);
        }
    }

    //해당 장비 타입의 아이템 정보를 패널로 표시합니다.
    private void OpenItemInfoPanel(EquipmentType _type)
    {
        if (equipmentManager == null || itemManager == null)
            return;

        EquipmentItem equipmentItem = equipmentManager.GetEquipmentItem(_type);
        if (equipmentItem != null)
        {
            ItemStack stack = itemManager.GetItemStack(equipmentItem);

            if (stack != null)
                creator.OpenPanel(stack);   //해당 아이템종류에 맞는 창을 열어 해당 아이템 정보를 뿌려줍니다.
        }
    }

    //인벤토리 창을 닫을때 호출됩니다.
    public void OnCloseInvenPanel()
    {
        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged -= OnEquip;
        }

        view.Initial();
    }

    //장비 변경 이벤트 처리
    private void OnEquip(EquipmentType _type, EquipmentItem _item)
    { 
        Equip(_type, _item);
    }

    //장비 아이템 드롭 성공 시 처리
    private void OnDropSuccessedEquipmentItem(EquipmentItem _item)
    {
        if (equipmentManager != null)
        {
            equipmentManager.Equip(_item.EquipmentType, _item);
        }
    }

    //강화 아이템 드롭 성공 시 처리
    private void OnDropSuccessedEnchantItem(EquipmentItem _equipment, EnchantItem _enchant)
    {
        if (equipmentManager != null && _equipment.EnchantPossibleAmount > 0)
        {
            view.Effect(_equipment.EquipmentType);
            equipmentManager.Enchant(_equipment, _enchant);
        }
    }

    private void OnDestroy()
    {
        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged -= OnEquip;
        }
    }
}
