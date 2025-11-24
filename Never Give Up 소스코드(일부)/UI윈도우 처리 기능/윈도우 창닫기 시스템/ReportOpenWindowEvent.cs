using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 윈도우가 열리거나 닫힐 때 이를 보고하는 이벤트를 관리하는 ScriptableObject입니다.
[CreateAssetMenu(fileName = "ReportOpenWindowEvent", menuName = "Scriptable Object/Event/ReportOpenWindowEvent")]
public class ReportOpenWindowEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    //윈도우 열림 이벤트
    private Action<IUIWindow> onReportOpenWindow;

    //윈도우 닫힘 이벤트
    private Action onReportCloseWindow;

    //윈도우 열림 이벤트 등록/해제
    public event Action<IUIWindow> OnReportOpenWindow
    {
        add { onReportOpenWindow += value; }
        remove { onReportOpenWindow -= value; }
    }

    //윈도우 닫힘 이벤트 등록/해제
    public event Action OnReportCloseWindow
    {
        add { onReportCloseWindow += value; }
        remove { onReportCloseWindow -= value; }
    }

    //윈도우 열림 이벤트 호출
    public void RaiseOpenWindow(IUIWindow _window) => onReportOpenWindow?.Invoke(_window);

    //윈도우 닫힘 이벤트 호출
    public void RaiseCloseWindow() => onReportCloseWindow?.Invoke();

    //초기화 (에디터상에서 플레이 시 에셋파일에 저장되므로 실행할때 반드시 호출해야합니다.)
    //빌드해서 플레이시 에셋파일에 저장되지 않기 때문에 필요없습니다.
    public override void Initial()
    {
        onReportCloseWindow = null;
        onReportOpenWindow = null;
    }
}
