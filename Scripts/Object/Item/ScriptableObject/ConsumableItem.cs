using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소비 아이템 정보를 담고 있는 클래스입니다.
/// 여러 기능 요소(ItemFunctionElement)를 포함하며, 사용 시 효과를 적용하고 사운드를 재생합니다.
/// </summary>
[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Item/ConsumableItem")]
public class ConsumableItem : Item
{
    [SerializeField] private List<ItemFunctionElement> itemFunctions;   //아이템 기능 요소 리스트
    [SerializeField] string useSfx; //아이템 사용 시 재생할 사운드 키값

    public override ItemType ItemType
    {
        get => ItemType.CONSUMABLE;
    }

    /// <summary>
    /// 아이템을 사용하여 효과를 적용합니다.
    /// </summary>
    public void Use(IItemUser _user)
    {
        if(!string.IsNullOrEmpty(useSfx))
            ServiceLocator.GetService<ISoundManager>()?.PlayOneShotSFX(useSfx);

        foreach (var itemFunction in itemFunctions)
            itemFunction.Use(_user);
    }
}
