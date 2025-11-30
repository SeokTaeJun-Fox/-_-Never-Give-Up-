using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnchantItemDataBase", menuName = "Scriptable Object/DataBase/EnchantItemDataBase")]
public class EnchantItemDataBase : ScriptableObject
{
    [SerializeField] private List<EnchantItem> enchantItemDatabase;

    public IReadOnlyList<EnchantItem> EnchantItemDatabase => enchantItemDatabase;
}
