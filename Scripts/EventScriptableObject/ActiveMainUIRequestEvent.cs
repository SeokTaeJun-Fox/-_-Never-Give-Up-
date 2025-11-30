using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메인 ui 활성화/비활성화 요청시 발생되는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "ActiveMainUIRequestEvent", menuName = "Scriptable Object/Event/ActiveMainUIRequestEvent")]
public class ActiveMainUIRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<bool> onActiveMainUIRequest;

    public event Action<bool> OnActiveMainUIRequest
    {
        add { onActiveMainUIRequest += value; }
        remove { onActiveMainUIRequest -= value; }
    }

    public void Raise(bool _isOn) => onActiveMainUIRequest?.Invoke(_isOn);

    public override void Initial()
    {
        onActiveMainUIRequest = null;
    }
}
