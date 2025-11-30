using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 생성 ㅇ요청시 발생하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "CreatePlayerEvent", menuName = "Scriptable Object/Event/CreatePlayerEvent")]
public class CreatePlayerRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Func<Vector3, Quaternion, PlayerController> onCreatePlayerRequest;

    public event Func<Vector3, Quaternion, PlayerController> OnCreatePlayerRequest
    {
        add { onCreatePlayerRequest += value; }
        remove { onCreatePlayerRequest -= value; }
    }

    public PlayerController RaiseEnter(Vector3 _pos, Quaternion _rot) => onCreatePlayerRequest?.Invoke(_pos, _rot);
    
    public override void Initial()
    {
        onCreatePlayerRequest = null;
    }

}
