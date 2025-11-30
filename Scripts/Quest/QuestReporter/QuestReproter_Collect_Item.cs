using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReproter_Collect_Item : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;
    [SerializeField] private Item collectTarget;

    private void Awake()
    {
        itemManager.OnGetItem += Report;
    }

    private void Report(Item _item, int _amount)
    {
        if(_item.ItemNum == collectTarget.ItemNum)
            QuestSystem.Instance.ReceiveReport(category, taskTarget, _amount);
    }
}
