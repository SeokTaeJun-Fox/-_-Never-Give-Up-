using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Scriptable Object/DataBase/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private List<Item> itemDatabase;
    
    public IReadOnlyList<Item> ItemDatabase => itemDatabase;
}
