using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItemDataBase", menuName = "Scriptable Object/DataBase/EquipmentItemDataBase")]
public class EquipmentItemDataBase : ScriptableObject
{
    [SerializeField] private List<EquipmentItem> eqeuipmentItemDatabase;

    public IReadOnlyList<EquipmentItem> EqeuipmentItemDatabase => eqeuipmentItemDatabase;
}
