using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_BuyItem : MonoBehaviour
{
    [SerializeField] private SelectedBuyItemPanelController controller;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;

    private void Awake()
    {
        controller.OnBuySuccessed += OnBuyItem;
    }

    public void OnBuyItem(int _amount)
    { 
        QuestSystem.Instance.ReceiveReport(category, taskTarget, _amount);
    }
}
