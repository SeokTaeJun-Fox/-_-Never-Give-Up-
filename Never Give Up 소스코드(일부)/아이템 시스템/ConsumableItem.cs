using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소비아이템 아이템을 정의하는 스크립터블 오브젝트 클래스입니다.
[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Item/ConsumableItem")]
public class ConsumableItem : Item
{
    [SerializeField] private List<ItemFunctionElement> itemFunctions;
    [SerializeField] string useSfx;

    public override ItemType ItemType
    {
        get => ItemType.CONSUMABLE;
    }

    public void Use(IItemUser _user)
    {
        if(!string.IsNullOrEmpty(useSfx))
            ServiceLocator.GetService<ISoundManager>()?.PlayOneShotSFX(useSfx);

        foreach (var itemFunction in itemFunctions)
            itemFunction.Use(_user);
    }
}
