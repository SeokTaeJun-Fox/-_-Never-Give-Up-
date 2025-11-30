using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//아이템 드랍 정보입니다.
[CreateAssetMenu(fileName = "ItemDropTable_", menuName = "Scriptable Object/Item/ItemDropTable")]
public class ItemDropTable : ScriptableObject
{
    [SerializeField] private List<itemDropInfo> itemDropInfos;

    public IReadOnlyList<itemDropInfo> ItemDropInfos => itemDropInfos;
}

[Serializable]
public struct itemDropInfo
{
    public Item item;
    public float probability;   //확률
    public int amountMin;       //아이템 1개 드랍시 최소 갯수
    public int amountMax;       //아이템 1개 드랍시 최대 갯수
    [SerializeField] private List<ConditionObject> conditions;

    public bool IsCondition => !conditions.Any() || conditions.All(x => x.IsPass());
}
