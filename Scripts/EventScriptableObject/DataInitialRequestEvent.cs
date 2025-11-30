using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 데이터 초기화 요청시 발생되는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "DataInitialRequestEvent", menuName = "Scriptable Object/Event/DataInitialRequestEvent")]
public class DataInitialRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action onDataInitialRequest;

    public event Action OnDataInitialRequest
    {
        add { onDataInitialRequest += value; }
        remove { onDataInitialRequest -= value; }
    }

    public void Raise() => onDataInitialRequest?.Invoke();

    public override void Initial()
    {
        onDataInitialRequest = null;
    }
}
