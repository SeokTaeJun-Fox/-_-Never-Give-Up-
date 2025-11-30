using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoaderEntry_", menuName = "Scriptable Object/SceneLoader/SceneLoaderEntry")]
public class SceneLoaderEntry : ScriptableObject
{
    [SerializeField] private LoadSceneType sceneType;
    [SerializeField] private int buildSceneIndex;

    public LoadSceneType SceneType { get => sceneType; }
    public int BuildSceneIndex { get => buildSceneIndex; }
}
