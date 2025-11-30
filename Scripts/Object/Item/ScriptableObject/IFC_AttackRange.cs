using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackRange_", menuName = "Scriptable Object/Item/ItemFunctionElement/AttackRange")]
public class IFC_AttackRange : ItemFunctionElement
{
    public override void Use(IItemUser _user)
    {
        if (_user != null)
            _user.ChangeAttackRange(amount);
    }

    public override void UnUse(IItemUser _user)
    {
        if (_user != null)
            _user.ChangeAttackRange(0);
    }
}
