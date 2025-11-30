using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MpRecovery_", menuName = "Scriptable Object/Item/ItemFunctionElement/MpRecovery")]
public class IFC_MpRecovery : ItemFunctionElement
{
    public override void Use(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseMp(amount);
    }

    public override void UnUse(IItemUser _user)
    {

    }
}
