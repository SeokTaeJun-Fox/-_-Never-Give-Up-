using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition_", menuName = "Scriptable Object/Condition/ConditionObject_CheckUnPossessItem")]
public class ConditionObject_CheckUnPossessItem : ConditionObject
{
    [SerializeField] private Item checkItem;

    public override bool IsPass()
    {
        return ServiceLocator.GetService<IItemManager>().GetItemStack(checkItem) == null;
    }
}
