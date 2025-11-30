using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장비 시스템의 핵심 기능을 정의하는 인터페이스입니다.
//장비의 장착, 강화, 조회, 초기화 및 관련 이벤트를 제공합니다.
public interface IEquipmentManager
{
    //현재 착용 중인 장비 정보 (장비 타입별로 저장된 딕셔너리)
    IDictionary<EquipmentType, EquipmentItem> EquippedItemDic { get; }

    //장비 변경 시 호출되는 이벤트 (장비 타입, 변경된 장비)
    event Action<EquipmentType, EquipmentItem> OnEquipChanged;

    //장비 강화 시 호출되는 이벤트 (강화된 장비, 사용된 강화 아이템)
    event Action<EquipmentItem, EnchantItem> OnEnchant;

    void Equip(EquipmentType _equipmentType, EquipmentItem _equipment);
    void Enchant(EquipmentItem _equipment, EnchantItem _enchant, bool isRemoveItem = true);
    EquipmentItem GetEquipmentItem(EquipmentType _equipmentType);

    void Initial();
}
