using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//"ISkillUser"를 받아오는 이벤트 클래스입니다.
[CreateAssetMenu(fileName = "GetISkillUserEvent", menuName = "Scriptable Object/Event/GetISkillUserEvent")]
public class GetISkillUserEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>"ISkillUser"를 받아오는 이벤트 클래스입니다.</summary>
    private Func<ISkillUser> onGet;

    public event Func<ISkillUser> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public ISkillUser RaiseGet() => onGet?.Invoke();

    public override void Initial()
    {
        onGet = null;
    }
}
