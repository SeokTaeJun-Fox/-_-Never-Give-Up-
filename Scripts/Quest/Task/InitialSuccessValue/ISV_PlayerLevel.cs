using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ISV는 InitialSuccessValue의 약자입니다.
[CreateAssetMenu(menuName = "Quest/Task/InitialSuccessValue/ISV_PlayerLevel", fileName = "ISV_PlayerLevel")]
public class ISV_PlayerLevel : InitialSuccessValue
{
    public override int GetValue(Task task)
    {
        var level = ServiceLocator.GetService<IPlayerAbilityManager>()?.PlayerAbilities[PlayerStat.LEVEL];
        if (level == null || !(level is int))
            return 0;
        else
            return (int)level;
    }
}
