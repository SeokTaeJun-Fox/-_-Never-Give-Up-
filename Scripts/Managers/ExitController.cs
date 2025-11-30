using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    [SerializeField] private UIWindowController windowController;
    [SerializeField] private RequestOpenPopupWindowEvent requestOpenPopupWindow;

    private void Awake()
    {
        windowController.OnInputKeyWhenEmpty += OpenExitPopup;
    }

    private void OpenExitPopup()
    {
        string title = "종료";
        string content = "게임을 종료하시겠습니까?";
        string yes = "예";
        string no = "아니오";

        requestOpenPopupWindow.Raise(title, content, true, yes, ExitGame, no, null);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if(windowController != null)
            windowController.OnInputKeyWhenEmpty -= OpenExitPopup;
    }
}
