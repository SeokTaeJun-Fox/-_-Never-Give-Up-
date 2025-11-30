using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoPanelEntry", menuName = "Scriptable Object/UI/ItemInfoPanelEntry")]
public class ItemInfoPanelEntry : ScriptableObject
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private GameObject prefab;

    //의존성 인터페이스 타입 이름 (예: IItemManager, IPlayerStatManager)
    [SerializeField] private string[] dependencyInterfaceTypeNames;

    public ItemType ItemType { get => itemType; }
    public GameObject Prefab { get => prefab; }
    public string[] DependencyInterfaceTypeNames { get => dependencyInterfaceTypeNames; }
}
