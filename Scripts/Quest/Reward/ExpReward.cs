using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Object/Reward/ExpReward")]
public class ExpReward : Reward
{
    public override void Give(Quest quest)
    {
        IPlayerAbilityManager playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();
        if (playerAbilityManager != null)
        {
            playerAbilityManager.AddExp(Quantity);
        }
    }
}
