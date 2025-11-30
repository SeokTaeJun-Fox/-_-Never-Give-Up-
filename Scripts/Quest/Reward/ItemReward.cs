using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Object/Reward/ItemReward")]
public class ItemReward : Reward
{
    [SerializeField] private Item item;
    [SerializeField] private bool isOverlap;

    public override Sprite Icon => item.Icon;
    public override string Description => item.ItemName;

    public override void Give(Quest quest)
    {
        IItemManager itemManager = ServiceLocator.GetService<IItemManager>();
        if (itemManager != null)
        {
            itemManager.GetItem(item, Quantity, !isOverlap);
        }
    }
}
