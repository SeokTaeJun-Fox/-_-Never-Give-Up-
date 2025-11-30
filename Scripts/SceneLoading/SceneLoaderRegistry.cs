using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoaderRegistry", menuName = "Scriptable Object/SceneLoader/SceneLoaderRegistry")]
public class SceneLoaderRegistry : ScriptableObject
{
    [SerializeField] private List<SceneLoaderEntry> entry;

    public List<SceneLoaderEntry> Entry { get => entry; }
}
