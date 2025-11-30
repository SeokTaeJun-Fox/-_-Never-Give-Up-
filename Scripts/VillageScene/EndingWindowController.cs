using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingWindowController : MonoBehaviour
{
    [SerializeField] private FadeRequestEvent fadeRequest;
    [SerializeField] private ShowUIWindowFactoryEvent uIWindowFactoryEvent;
    [SerializeField] private OnCloseRewardPanelEvent onCloseRewardPanelEvent;
    [SerializeField] private ActiveMainUIRequestEvent activeMainUIRequestEvent;
    [SerializeField] private GetPlayerControllerEvent playerControllerEvent;
    [SerializeField] private Quest endingTargetQuest;

    private void Awake()
    {
        QuestSystem.Instance.onQuestCompleted += CheckCompleteTargetQuest;
    }

    public void CheckCompleteTargetQuest(Quest _quest)
    {
        if (_quest.CodeName == endingTargetQuest.CodeName)
        {
            onCloseRewardPanelEvent.OnClose += ShowEndingPanel;//다이얼로그 컨트롤러에 이벤트 추가
        }
    }

    private void ShowEndingPanel()
    {
        activeMainUIRequestEvent.Raise(false);
        playerControllerEvent.RaiseGet()?.SetCanControl(false);
        fadeRequest.RaiseFadeIn(OpenEndingPanel);
    }

    private void OpenEndingPanel()
    { 
        EndingPanelModel model = new EndingPanelModel(FadeOut);
        uIWindowFactoryEvent.Raise(UIType.ENDING_WINDOW, new object[] { model });
    }

    private void FadeOut()
    {
        fadeRequest.RaiseFadeOut(InitialBack);
    }

    private void InitialBack()
    {
        playerControllerEvent.RaiseGet()?.SetCanControl(true);
        activeMainUIRequestEvent.Raise(true);
    }

    private void OnDestroy()
    {
        if (onCloseRewardPanelEvent != null)
        {
            onCloseRewardPanelEvent.OnClose -= ShowEndingPanel;
        }
        if (QuestSystem.Instance)
            QuestSystem.Instance.onQuestCompleted -= CheckCompleteTargetQuest;
    }
}
