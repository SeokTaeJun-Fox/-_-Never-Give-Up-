using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoPanelRegistry", menuName = "Scriptable Object/UI/ItemInfoPanelRegistry")]
public class ItemInfoPanelRegistry : ScriptableObject
{
    [SerializeField] private List<ItemInfoPanelEntry> entries;

    public List<ItemInfoPanelEntry> Entries { get => entries; }
}
