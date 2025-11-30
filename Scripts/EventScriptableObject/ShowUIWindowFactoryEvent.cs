using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uiWindow 생성담당클래스(UIWindowFactoryEvent)에 윈도우 생성을 요청하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "ShowUIWindowFactoryEvent", menuName = "Scriptable Object/Event/ShowUIWindowFactoryEvent")]
public class ShowUIWindowFactoryEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>윈도우 생성 요청시 실행하는 이벤트입니다.</summary>
    private Action<UIType, object[]> onShowWindow;

    public event Action<UIType, object[]> OnShowWindow
    {
        add { onShowWindow += value; }
        remove { onShowWindow -= value; }
    }
    public void Raise(UIType _uiType, object[] _initialDatas = null) => onShowWindow?.Invoke(_uiType, _initialDatas);

    public override void Initial()
    {
        onShowWindow = null;
    }
}
