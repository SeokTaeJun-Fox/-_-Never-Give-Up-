using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddMaxMp_", menuName = "Scriptable Object/Item/ItemFunctionElement/AddMaxMp")]
public class IFC_AddMaxMp : ItemFunctionElement
{
    public override void Use(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddMaxMp(amount);
    }

    public override void UnUse(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddMaxMp(-amount);
    }
}
