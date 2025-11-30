using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_SkillEnchant : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;

    private void Awake()
    {
        skillManager.OnSkillLevelUp += Report; 
    }

    private void Report(Skill _skill)
    {
        QuestSystem.Instance.ReceiveReport(category, taskTarget, 1);
    }

}
