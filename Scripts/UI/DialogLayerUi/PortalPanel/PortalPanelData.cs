using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "PortalPanelData_", menuName = "Scriptable Object/Portal/PortalPanelData")]
public class PortalPanelData : ScriptableObject
{
    [SerializeField] private List<PortalPanelElementData> datas;

    public IReadOnlyList<PortalPanelElementData> Datas => datas;
}

[Serializable]
public class PortalPanelElementData
{
    [SerializeField] private string buttonText;
    [SerializeField] private int mapId;
    [SerializeField] private LoadSceneType loadSceneType;
    [SerializeField] private ConditionObject[] activeConditions;

    public string ButtonText { get => buttonText; }
    public int MapId { get => mapId; }
    public LoadSceneType LoadSceneType { get => loadSceneType; }
    public ConditionObject[] ActiveConditions { get => activeConditions; }

    public bool IsCondition => !activeConditions.Any() || activeConditions.All(x => x.IsPass());
}
