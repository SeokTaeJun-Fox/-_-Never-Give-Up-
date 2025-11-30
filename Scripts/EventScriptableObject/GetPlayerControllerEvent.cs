using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//플레이어 컨트롤러 반환 요청시 발생하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "GetPlayerControllerEvent", menuName = "Scriptable Object/Event/GetPlayerControllerEvent")]
public class GetPlayerControllerEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>플레이어 컨트롤러 반환요청시 실행됩니다.</summary>
    private Func<PlayerController> onGet;

    public event Func<PlayerController> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public PlayerController RaiseGet() => onGet?.Invoke();

    public override void Initial()
    {
        onGet = null;
    }
}
