using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveItemAction", menuName = "Scriptable Object/ActionObject/RemoveItemAction")]
public class RemoveItemAction : ActionObject
{
    [SerializeField] private Item target;
    [SerializeField] private int amount;

    public override void Action()
    {
        IItemManager itemManager = ServiceLocator.GetService<IItemManager>();
        if (itemManager != null)
        {
            ServiceLocator.GetService<IItemManager>().RemoveItem(target, amount);
        }
    }
}
