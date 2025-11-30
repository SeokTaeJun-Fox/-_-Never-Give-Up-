using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DialogPanelManager : MonoBehaviour
{
    //이 클래스에 필요한 스크립터블 오브젝트
    [SerializeField] private ShowUIWindowFactoryEvent showUIWindowFactoryEvent;
    [SerializeField] private OpenDialogPanelRequestEvent openDialogPanelRequestEvent;
    [SerializeField] private OnCloseRewardPanelEvent onCloseRewardPanelEvent;

    private List<NpcQuest> curQuestList;
    private Quest targetQuest;
    
    public event Action OnCloseRewardPanel;

    private void Awake()
    {
        if (openDialogPanelRequestEvent != null)
        {
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow += OpenDialogPanel;
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow2 += OpenDialogPanel;
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow3 += OpenDialogAndComplete;
        }
    }

    //다이얼로그만 띄웁니다.
    public void OpenDialogPanel(Dialogue _dialog)
    {
        DialogPanelModel model = new DialogPanelModel(_dialog, onCloseDialogPanelButton, false);

        //activeMainUIRequestEvent?.Raise(false);
        showUIWindowFactoryEvent?.Raise(UIType.DIALOG_WINDOW, new object[] { model });
    }

    //다이얼로그 + 퀘스트리스트를 보여줍니다.
    public void OpenDialogPanel(Dialogue _dialog, List<NpcQuest> _questList)
    {
        curQuestList = _questList;
        DialogPanelModel model = new DialogPanelModel(_dialog, onCloseDialogPanelButton, true, onClickDialogQuestButton);

        //activeMainUIRequestEvent?.Raise(false);
        showUIWindowFactoryEvent?.Raise(UIType.DIALOG_WINDOW, new object[] { model });
    }

    //다이얼로그 + 퀘스트를 완료합니다.
    public void OpenDialogAndComplete(Dialogue _dialog, Quest _completeQuest)
    {
        targetQuest = _completeQuest;
        DialogPanelModel model = new DialogPanelModel(_dialog, onCloseDialogPanelAndCompleteQuest, false);

        //activeMainUIRequestEvent?.Raise(false);
        showUIWindowFactoryEvent?.Raise(UIType.DIALOG_WINDOW, new object[] { model });
    }

    //이벤트
    private void onCloseDialogPanelButton()
    {
        //activeMainUIRequestEvent?.Raise(true);
    }

    private void onCloseRewardPanelButton()
    {
        //activeMainUIRequestEvent?.Raise(true);
        onCloseRewardPanelEvent.RaiseOnClose();
    }

    private void onCloseDialogPanelAndCompleteQuest()
    {
        //activeMainUIRequestEvent?.Raise(true);
        if (targetQuest != null)
        {
            targetQuest.Complete();

            //보상ui 열기
            if (targetQuest.Rewards.Count > 0)
            {
                RewardWindowModel model = new RewardWindowModel(targetQuest.Rewards.ToList(), onCloseRewardPanelButton);
                showUIWindowFactoryEvent?.Raise(UIType.REWARD_WINDOW, new object[] { model });
            }
            //else
                //activeMainUIRequestEvent?.Raise(true);

            targetQuest = null;
        }
    }

    //다이얼로그 퀘스트 버튼 클릭시 실행
    private void onClickDialogQuestButton()
    {
        QuestListWindowModel model = new QuestListWindowModel(curQuestList, OnClickElementButton);
        showUIWindowFactoryEvent?.Raise(UIType.QUEST_LIST_WINDOW, new object[] { model });
    }

    //퀘스트 리스트버튼 클릭시 실행
    private void OnClickElementButton(NpcQuest _npcQuest)
    {
        if (_npcQuest == null || _npcQuest.IsRegistered)
            return;

        QuestSystem.Instance.Register(_npcQuest);   //서비스 로케이터로 이동시 수정예정

        OpenDialogPanel(_npcQuest.RequestDialog);
    }

    private void OnDestroy()
    {
        if (openDialogPanelRequestEvent != null)
        {
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow -= OpenDialogPanel;
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow2 -= OpenDialogPanel;
            openDialogPanelRequestEvent.OnRequestOpenDialogWindow3 -= OpenDialogAndComplete;
        }
    }
}
