using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//플레이어가 아이템 접근시, 벗어날시 발생하는 이벤트 처리 클래스입니다.
[CreateAssetMenu(fileName = "ItemProximityEvent", menuName = "Scriptable Object/Event/ItemProximityEvent")]
public class ItemProximityEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>아이템 범위에 플레이어가 들어올때 발생됩니다.</summary>
    private Action<Item3D> onEnter;
    /// <summary>아이템 범위에서 플레이어가 나갈때 발생됩니다.</summary>
    private Action<Item3D> onExit;

    public event Action<Item3D> OnEnter
    { 
        add { onEnter += value; }
        remove { onEnter -= value; }
    }

    public event Action<Item3D> OnExit
    {
        add { onExit += value; }
        remove { onExit -= value; }
    }

    public void RaiseEnter(Item3D _item) => onEnter?.Invoke(_item);
    public void RaiseExit(Item3D _item) => onExit?.Invoke(_item);

    public override void Initial()
    {
        onEnter = null;
        onExit = null;
    }
}
