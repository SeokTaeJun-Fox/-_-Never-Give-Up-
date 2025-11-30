using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//씬 로드 요청시 발생하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "LoadSceneRequestEvent", menuName = "Scriptable Object/Event/LoadSceneRequestEvent")]
public class LoadSceneRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<LoadSceneType, int> onSceneLoadRequest;

    public event Action<LoadSceneType, int> OnSceneLoadRequest
    {
        add { onSceneLoadRequest += value; }
        remove { onSceneLoadRequest -= value; }
    }

    public void Raise(LoadSceneType _loadSceneType, int _mapId) => onSceneLoadRequest?.Invoke(_loadSceneType, _mapId);

    public override void Initial()
    {
        onSceneLoadRequest = null;
    }
}
