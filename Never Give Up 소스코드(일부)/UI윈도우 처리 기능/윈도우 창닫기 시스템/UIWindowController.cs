using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// UI 윈도우의 열림/닫힘을 관리하고 ESC 키 입력 시 창을 닫는 기능을 제공하는 컨트롤러입니다.
public class UIWindowController : MonoBehaviour
{
    [SerializeField] private ReportOpenWindowEvent reportOpenWindowEvent;   //윈도우 열림/닫힘 이벤트
    [SerializeField] private OnActiveWindowCountChangedEvent activeWindowCountChangedEvent; //활성 윈도우 수 변경 이벤트
    [SerializeField] private Stack<IUIWindow> activeWindows = new Stack<IUIWindow>();   //현재 열린 윈도우 스택
    public event Action OnInputKeyWhenEmpty;    //열린 윈도우가 없을때 esc를 누르면 이벤트 발생됩니다.

    private void Awake()
    {
        //이벤트 등록
        reportOpenWindowEvent.OnReportOpenWindow += ReportOpenWindow;
        reportOpenWindowEvent.OnReportCloseWindow += ReportCloseWindow;
    }

    private void Update()
    {     
        //ESC 키 입력시 윈도우 닫기 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeWindows.Count > 0)
            {
                IUIWindow closeWindow = activeWindows.Pop();
                closeWindow.Close();

                activeWindowCountChangedEvent?.RaiseEvent(activeWindows.Count);
            }
            else
            {
                OnInputKeyWhenEmpty?.Invoke();
            }
        }
    }

    //윈도우 열림 보고
    public void ReportOpenWindow(IUIWindow _window)
    {
        activeWindows.Push(_window);
        activeWindowCountChangedEvent?.RaiseEvent(activeWindows.Count);
    }

    //윈도우 딛힘 보고
    public void ReportCloseWindow()
    {
        activeWindows.Pop();
        activeWindowCountChangedEvent?.RaiseEvent(activeWindows.Count);
    }

    private void OnDestroy()
    {
        //이벤트 해제
        reportOpenWindowEvent.OnReportOpenWindow -= ReportOpenWindow;
        reportOpenWindowEvent.OnReportCloseWindow -= ReportCloseWindow;
    }
}
