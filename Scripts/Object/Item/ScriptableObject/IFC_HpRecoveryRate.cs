using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HpRecoveryRate_", menuName = "Scriptable Object/Item/ItemFunctionElement/HpRecoveryRate")]
public class IFC_HpRecoveryRate : ItemFunctionElement
{
    public override void Use(IItemUser _user)
    {
        if (_user != null)
        {
            int recoveryAmount = (int)(((int)_user.PlayerAbilities[PlayerStat.TOTAL_MAXHP]) * amount);
            _user.IncreaseHp(recoveryAmount);
        }
    }

    public override void UnUse(IItemUser _user)
    {

    }
}
