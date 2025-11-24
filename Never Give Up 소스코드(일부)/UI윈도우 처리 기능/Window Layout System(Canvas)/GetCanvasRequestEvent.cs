using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 윈도우가 생성될 캔버스를 요청할 때 사용하는 이벤트 ScriptableObject입니다.
// UIWindowFactory에서 캔버스 위치를 결정할 때 사용됩니다.
[CreateAssetMenu(fileName = "GetCanvasRequestEvent", menuName = "Scriptable Object/Event/GetCanvasRequestEvent")]
public class GetCanvasRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Func<CanvasType, GameObject> onGet; //캔버스 요청 이벤트 핸들러

    public event Func<CanvasType, GameObject> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public GameObject RaiseGet(CanvasType _nameKey) => onGet?.Invoke(_nameKey);

    public override void Initial()
    {
        onGet = null;
    }
}
