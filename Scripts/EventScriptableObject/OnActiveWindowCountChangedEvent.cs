using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 활성화된 UI 윈도우의 개수가 변경될 때 발생하는 이벤트를 관리하는 ScriptableObject입니다.
/// </summary>
[CreateAssetMenu(fileName = "OnActiveWindowCountChangedEvent", menuName = "Scriptable Object/Event/OnActiveWindowCountChangedEvent")]
public class OnActiveWindowCountChangedEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    //윈도우 수 변경 이벤트
    private Action<int> onActiveWindowCountChanged;

    //이벤트 등록/해제
    public event Action<int> OnActiveWindowCountChanged
    {
        add { onActiveWindowCountChanged += value; }
        remove { onActiveWindowCountChanged -= value; }
    }

    //이벤트 호출
    public void RaiseEvent(int _count) => onActiveWindowCountChanged?.Invoke(_count);

    public override void Initial()
    {
        onActiveWindowCountChanged = null;
    }
}
