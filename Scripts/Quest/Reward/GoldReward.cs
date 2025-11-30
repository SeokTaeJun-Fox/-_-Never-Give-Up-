using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Object/Reward/GoldReward")]
public class GoldReward : Reward
{
    public override void Give(Quest quest)
    {
        IItemManager itemManager = ServiceLocator.GetService<IItemManager>();
        if (itemManager != null)
        {
            itemManager.GetGold(Quantity);
        }
    }
}
