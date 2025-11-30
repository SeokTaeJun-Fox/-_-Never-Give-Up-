using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

//대화창의 데이터를 관리하는 모델 클래스입니다.
//현재 대사 인덱스, 대사 진행, 버튼 패널 상태, 퀘스트 이벤트 등을 포함하며,
//컨트롤러에 이벤트를 통해 대사 변경을 전달합니다.
public class DialogPanelModel
{
    private Dialogue dialog;
    private Action onClose;
    private bool isButtonPanelOpen;
    private Action onClickQuestButton;

    public Action OnClose { get => onClose; }
    public bool IsButtonPanelOpen { get => isButtonPanelOpen; }
    public Action OnClickQuestButton { get => onClickQuestButton; }

    public DialogPanelModel(Dialogue _dialog, Action _onClose, bool _isButtonPanelOpen)
    {
        dialog = _dialog;
        onClose = _onClose;
        isButtonPanelOpen = _isButtonPanelOpen;
    }

    public DialogPanelModel(Dialogue _dialog, Action _onClose, bool _isButtonPanelOpen, Action _onClickQuestButton)
    {
        dialog = _dialog;
        onClose = _onClose;
        isButtonPanelOpen = _isButtonPanelOpen;
        onClickQuestButton = _onClickQuestButton;
    }

    //다이얼로그 영역
    public event Action<DialogueLine> OnLineChanged;

    private int curLineIndex = 0;

    //다이얼로그 정보형
    //특정 인덱스의 대사를 설정하고 이벤트를 통해 전달합니다.
    public void SetLine(int _lineIndex)
    {
        if (_lineIndex < dialog.Lines.Count)
        {
            curLineIndex = _lineIndex;
            OnLineChanged?.Invoke(dialog.Lines[curLineIndex]);
        }
        else
        {
            Debug.LogWarning($"다이얼로그 라인 설정 {_lineIndex}값이 다이얼로그 리스트의 인덱스값을 넘어섰습니다.");
        }
    }

    //다음 대사로 진행하고 이벤트를 통해 전달합니다.
    public void NextLine()
    {
        if (curLineIndex + 1 < dialog.Lines.Count)
        {
            curLineIndex++;
            OnLineChanged?.Invoke(dialog.Lines[curLineIndex]);
        }
        else
        {
            Debug.LogWarning($"다이얼로그 라인 설정 {curLineIndex}값이 다이얼로그 리스트의 인덱스값을 넘어섰습니다.");
        }
    }

    //현재 대사 인덱스를 반환합니다.
    public int GetCurLineIndex()
    {
        return curLineIndex;
    }

    //전체 대사 수를 반환합니다.
    public int GetDialogCount()
    {
        return dialog.Lines.Count;
    }

    //현재 대사가 마지막인지 여부를 반환합니다.
    public bool IsEndLine()
    {
        return (curLineIndex + 1) == dialog.Lines.Count;
    }
}
