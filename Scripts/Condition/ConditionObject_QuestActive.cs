using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition_", menuName = "Scriptable Object/Condition/ConditionObject_QuestActive")]
public class ConditionObject_QuestActive : ConditionObject
{
    [SerializeField] private Quest target;

    public override bool IsPass()
    {
        return QuestSystem.Instance.ContainsInActiveQuests(target);
    }
}
