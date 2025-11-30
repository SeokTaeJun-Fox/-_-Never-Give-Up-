using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_MonsterKill : MonoBehaviour
{
    [SerializeField] private TaskTarget target;
    [SerializeField] private Category category;
    [SerializeField] private int amount;

    public void OnDead()
    {
        QuestSystem.Instance.ReceiveReport(category, target, amount);
    }
}
