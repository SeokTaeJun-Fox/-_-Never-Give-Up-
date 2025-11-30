using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다이얼로그 매니저 에게 다이얼로그오픈을 요청하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "OpenDialogPanelRequestEvent", menuName = "Scriptable Object/Event/OpenDialogPanelRequestEvent")]
public class OpenDialogPanelRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<Dialogue> onRequestOpenDialogWindow;
    private Action<Dialogue, List<NpcQuest>> onRequestOpenDialogWindow2;
    private Action<Dialogue, Quest> onRequestOpenDialogWindow3;

    public event Action<Dialogue> OnRequestOpenDialogWindow
    {
        add { onRequestOpenDialogWindow += value; }
        remove { onRequestOpenDialogWindow -= value; }
    }

    public event Action<Dialogue, List<NpcQuest>> OnRequestOpenDialogWindow2
    {
        add { onRequestOpenDialogWindow2 += value; }
        remove { onRequestOpenDialogWindow2 -= value; }
    }

    public event Action<Dialogue, Quest> OnRequestOpenDialogWindow3
    {
        add { onRequestOpenDialogWindow3 += value; }
        remove { onRequestOpenDialogWindow3 -= value; }
    }

    public void Raise(Dialogue _dialog) => onRequestOpenDialogWindow?.Invoke(_dialog);
    public void Raise(Dialogue _dialog, List<NpcQuest> _questList) => onRequestOpenDialogWindow2?.Invoke(_dialog, _questList);
    public void Raise(Dialogue _dialog, Quest _completeQuest) => onRequestOpenDialogWindow3?.Invoke(_dialog, _completeQuest);

    public override void Initial()
    {
        onRequestOpenDialogWindow = null;
        onRequestOpenDialogWindow2 = null;
        onRequestOpenDialogWindow3 = null;
    }
}
