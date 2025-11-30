using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition_", menuName = "Scriptable Object/Condition/ConditionObject_QuestComplete")]
public class ConditionObject_QuestComplete : ConditionObject
{
    [SerializeField] private Quest target;

    public override bool IsPass()
    => QuestSystem.Instance.ContainsInCompleteQuests(target);
}
