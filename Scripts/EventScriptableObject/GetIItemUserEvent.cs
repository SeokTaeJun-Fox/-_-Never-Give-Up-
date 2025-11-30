using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//"IItemUser"를 받아오는 이벤트 클래스입니다.
[CreateAssetMenu(fileName = "GetIItemUserEvent", menuName = "Scriptable Object/Event/GetIItemUserEvent")]
public class GetIItemUserEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>"IItemUser"를 받아오는 이벤트 클래스입니다.</summary>
    private Func<IItemUser> onGet;

    public event Func<IItemUser> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public IItemUser RaiseGet() => onGet?.Invoke();

    public override void Initial()
    {
        onGet = null;
    }
}
