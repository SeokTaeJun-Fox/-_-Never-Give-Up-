using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보상 패널 닫을시 발생하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "OnCloseRewardPanelEvent", menuName = "Scriptable Object/Event/OnCloseRewardPanelEvent")]
public class OnCloseRewardPanelEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>보상 패널 닫을시 발생됩니다.</summary>
    private Action onClose;

    public event Action OnClose
    {
        add { onClose += value; }
        remove { onClose -= value; }
    }

    public void RaiseOnClose() => onClose?.Invoke();

    public override void Initial()
    {
        onClose = null;
    }
}
