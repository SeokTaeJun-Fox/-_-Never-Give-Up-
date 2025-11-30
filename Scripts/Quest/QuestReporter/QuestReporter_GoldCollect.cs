using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_GoldCollect : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;

    private void Awake()
    {
        itemManager.OnGetGold += OnGetGold;
    }

    private void OnGetGold(int _amount)
    { 
        QuestSystem.Instance.ReceiveReport(category, taskTarget, _amount);
    }
}
