using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnInfoRegister_", menuName = "Scriptable Object/MonsterSpawn/MonsterSpawnInfoRegister")]
public class MonsterSpawnInfoRegister : ScriptableObject
{
    [SerializeField] private List<MonsterSpawnInfoEntry> entry;

    public List<MonsterSpawnInfoEntry> Entry { get => entry; }
}
