using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MpRecoveryRate_", menuName = "Scriptable Object/Item/ItemFunctionElement/MpRecoveryRate")]
public class IFC_MpRecoveryRate : ItemFunctionElement
{
    public override void Use(IItemUser _user)
    {
        if (_user != null)
        {
            int recoveryAmount = (int)(((int)_user.PlayerAbilities[PlayerStat.TOTAL_MAXMP]) * amount);
            _user.IncreaseMp(recoveryAmount);
        }
    }

    public override void UnUse(IItemUser _user)
    {

    }
}
