using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//능력치 변화요청이벤트 클래스입니다.
[CreateAssetMenu(fileName = "RequestChangeStatEvent", menuName = "Scriptable Object/Event/RequestChangeStatEvent")]
public class RequestChangeStatEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>능력치 변화요청시 발생됩니다.</summary>
    private Action<int> onRequestChangeStat;

    public event Action<int> OnRequestChangeStat
    {
        add { onRequestChangeStat += value; }
        remove { onRequestChangeStat -= value; }
    }

    public void RaiseOnRequestChangeStat(int _stat) => onRequestChangeStat?.Invoke(_stat);

    public override void Initial()
    {
        onRequestChangeStat = null;
    }
}
