using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_PlayerLevel : MonoBehaviour
{
    [SerializeField] private PlayerAbilityManager playerAbilityManager;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;

    private void Awake()
    {
        playerAbilityManager.OnLevelUp += OnLevelup;
    }

    private void OnLevelup(int _level)
    {
        QuestSystem.Instance.ReceiveReport(category, taskTarget, _level);
    }
}
