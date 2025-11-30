using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickQuestPanel : MonoBehaviour
{
    [SerializeField] private QuickQuestElement preb;
    [SerializeField] private Transform prebParent;

    private List<QuickQuestElement> elements = new();

    private void Awake()
    {
        foreach (var quest in QuestSystem.Instance.ActiveQuests)
        {
            var element = Instantiate(preb, prebParent);
            element.Setting(quest);
            elements.Add(element);
        }

        QuestSystem.Instance.onQuestRegistered += OnQuestRegistered;
        QuestSystem.Instance.onQuestInitial += RemoveAllQuestElementList;
    }

    private void OnQuestRegistered(Quest _newQuest)
    {
        var element = Instantiate(preb, prebParent);
        element.Setting(_newQuest);
        elements.Add(element);
    }

    private void RemoveAllQuestElementList()
    {
        foreach (var element in elements)
        {
            if(element != null)
                Destroy(element.gameObject);
        }

        elements.Clear();
    }

    private void OnDestroy()
    {
        if (QuestSystem.Instance)
        {
            QuestSystem.Instance.onQuestRegistered -= OnQuestRegistered;
            QuestSystem.Instance.onQuestInitial -= RemoveAllQuestElementList;
        }
    }
}
