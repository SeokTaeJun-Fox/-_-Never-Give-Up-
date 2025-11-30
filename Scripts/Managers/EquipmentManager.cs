using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 장비를 관리하는 클래스입니다.
//장비의 장착/해제, 강화 처리, 이벤트 호출 등을 담당합니다.
public class EquipmentManager : MonoBehaviour, IEquipmentManager
{
    private IItemManager itemManager;

    //현재 착용 중인 장비 정보 (장비 타입별로 저장)
    private Dictionary<EquipmentType, EquipmentItem> equippedItemDic = new Dictionary<EquipmentType, EquipmentItem>();
    public IDictionary<EquipmentType, EquipmentItem> EquippedItemDic => equippedItemDic;

    //장비 변경 시 발생하는 이벤트
    public event Action<EquipmentType, EquipmentItem> OnEquipChanged;

    //장비 강화 시 발생하는 이벤트
    public event Action<EquipmentItem, EnchantItem> OnEnchant;

    //장비 효과 적용 대상 요청 이벤트
    [SerializeField] private GetIItemUserEvent getIItemUserEvent;

    private void Awake()
    {
        Initial();

        ServiceLocator.Register<IEquipmentManager>(this);
    }

    private void Start()
    {
        itemManager = ServiceLocator.GetService<IItemManager>();
    }

    //장비를 장착합니다. 기존 장비는 해제되고 새 장비가 적용됩니다.
    public void Equip(EquipmentType _equipmentType, EquipmentItem _equipment)
    {
        UnEquip(_equipmentType);

        equippedItemDic[_equipmentType] = _equipment;
        if (_equipment != null)
            _equipment.Equip(getIItemUserEvent.RaiseGet());

        OnEquipChanged?.Invoke(_equipmentType, equippedItemDic[_equipmentType]);
    }

    //장비를 해제합니다.
    private void UnEquip(EquipmentType _equipmentType)
    {
        if (equippedItemDic.ContainsKey(_equipmentType) && equippedItemDic[_equipmentType] != null)
        {
            equippedItemDic[_equipmentType].UnEquip(getIItemUserEvent.RaiseGet());    //임시
            equippedItemDic[_equipmentType] = null;
        }
    }

    //장비에 강화 아이템을 적용합니다.
    //강화 가능 횟수가 남아있고, 필요 시 인벤토리에서 강화 아이템을 제거합니다.
    //로드 시에는 isRemoveItem을 false로 설정하여 아이템을 제거하지 않습니다.
    public void Enchant(EquipmentItem _equipment, EnchantItem _enchant, bool isRemoveItem = true)
    {
        if (_equipment.EnchantPossibleAmount > 0)
        {
            if (itemManager != null)
            {
                //해당 강화아이템을 제거해야한다면
                if (isRemoveItem)
                    itemManager.RemoveItem(_enchant, 1);

                _equipment.Enchant(_enchant, getIItemUserEvent.RaiseGet());   //임시
                OnEnchant?.Invoke(_equipment, _enchant);
            }
        }
    }

    //특정 장비 타입의 현재 착용 장비를 반환합니다.
    public EquipmentItem GetEquipmentItem(EquipmentType _equipmentType)
    {
        if (equippedItemDic.ContainsKey(_equipmentType))
            return equippedItemDic[_equipmentType];
        else 
            return null;
    }

    //장비 정보를 초기화합니다. 모든 장비 타입을 null로 설정합니다.
    //로비 화면으로 돌아갈 때 호출됩니다.
    public void Initial()
    {
        equippedItemDic.Clear();

        //딕셔너리 초깃값 세팅
        foreach (EquipmentType equipmentType in Enum.GetValues(typeof(EquipmentType)))
        {
            equippedItemDic.Add(equipmentType, null);
        }
    }
}
