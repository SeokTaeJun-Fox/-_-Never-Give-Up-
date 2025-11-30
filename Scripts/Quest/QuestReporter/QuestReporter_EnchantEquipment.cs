using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter_EnchantEquipment : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;

    [SerializeField] private Category category;
    [SerializeField] private TaskTarget taskTarget;

    private void Awake()
    {
        equipmentManager.OnEnchant += Report;
    }

    private void Report(EquipmentItem _item, EnchantItem _enchantItem)
    {
        QuestSystem.Instance.ReceiveReport(category, taskTarget, 1);
    }
}
