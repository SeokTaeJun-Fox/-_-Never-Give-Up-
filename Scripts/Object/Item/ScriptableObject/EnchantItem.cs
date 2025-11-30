using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 강화 아이템 정보를 담고 있는 클래스입니다.
// 장비에 증력치를 추가하거나 교체하는 데 사용되며, ItemFunctionElement 기반으로 효과를 구성합니다.
[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Item/EnchantItem")]
public class EnchantItem : Item
{
    public override ItemType ItemType
    {
        get => ItemType.ENCHANT;
    }

    [SerializeField] private List<ItemFunctionElement> abilities;   //강화 능력치 리스트
    public List<ItemFunctionElement> Abilities { get => abilities; }
    
    //대상에게 강화 능력치를 적용합니다.
    public void Use(IItemUser _user)
    {
        foreach (var ability in abilities)
            ability.Use(_user);
    }
}
