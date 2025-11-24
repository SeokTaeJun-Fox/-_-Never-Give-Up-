using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//장비 슬롯 UI에 붙여있는 드롭 수신기입니다.
//장비 아이템 또는 강화 아이템이 드롭될 때 조건을 확인하고 이벤트를 발생시킵니다.
public class EquipmentItemDropReceiver : ItemDropReceiver
{
    [SerializeField] private EquipmentType equipmentType;   //이 슬롯이 허용하는 장비 타입

    //이 클래스의 이벤트 발생조건은 드롭될 아이템이 장비형아이템이고,
    //이 칸에 넣을수있는 장비종류와 드롭될 아이템의 장비종류가 같을때 발생됩니다.
    //또한 드롭될 아이템과 현재 이 칸에 등록된 아이템이 달라야 합니다.

    public event Action<EquipmentItem> OnDropSuccessedEquipmentItem;    //장비 아이템 드롭 성공 이벤트
    public event Action<EquipmentItem, EnchantItem> OnDropSuccessedEnchantItem; //강화 아이템 드롭 성공 이벤트

    public EquipmentItem thisItem;  //현재 슬롯에 등록된 장비 아이템
    public EquipmentType thisEquipmentType => equipmentType;

    //아이템 드롭 시 조건을 확인하고 이벤트를 발생시킵니다.
    public override void DropItem(Item _item)
    {
        //드롭 아이템이 강화형 아이템일때 이 칸에 등록된 장비아이템이 있으면 발생됩니다.
        EquipmentItem equipmentItem = _item as EquipmentItem;
        EnchantItem enchantItem = _item as EnchantItem;

        //강화 아이템 드롭 처리
        if (enchantItem != null && thisItem != null)
        {
            OnDropSuccessedEnchantItem(thisItem, enchantItem);
            return;
        }

        //장비 아이템 드롭 처리
        if (equipmentItem != null && equipmentItem.EquipmentType == equipmentType &&
            !((thisItem != null) && thisItem.ItemKey == equipmentItem.ItemKey))
            OnDropSuccessedEquipmentItem(equipmentItem);
    }
}
