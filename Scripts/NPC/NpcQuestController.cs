using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcQuestController : MonoBehaviour
{
    [SerializeField] private List<NpcQuest> giveQuests; //npc 요청 퀘스트 목록들
    [SerializeField] private List<NpcQuest> takeQuests; //이 npc가 완료할 퀘스트 목록들

    //npc ui 뷰
    [SerializeField] private NpcCanvas npcCanvas;

    //현재 퀘스트 상태 및 데이터
    [SerializeField] private List<NpcQuest> curReadyQuests;  //현재 대기중인 퀘스트
    [SerializeField] private NpcQuest inProgressQuest;   //현재 진행중인 퀘스트
    [SerializeField] private NpcQuest completeReadyTakeQuest;    //완료 대기중인 takeQuest
    private NpcQuestState curState;    //현재 npc퀘스트 상태

    [SerializeField] private string npcCode;

    public IReadOnlyList<NpcQuest> CurReadyQuests { get => curReadyQuests;  }
    public NpcQuest InProgressQuest { get => inProgressQuest; }
    public NpcQuest CompleteReadyTakeQuest { get => completeReadyTakeQuest; }
    public NpcQuestState CurState { get => curState; }

    private void Awake()
    {
        QuestSystem.Instance.onQuestRegistered += OnQuestRegistered;
        QuestSystem.Instance.onQuestCompletedReady += OnQuestCompletedReady;
        QuestSystem.Instance.onQuestCompleted += OnQuestCompleted;
        QuestSystem.Instance.onQuestCanceled += OnQuestCanceled;

        Initial();
        Setting();
    }

    private void Initial()
    {
        giveQuests = QuestSystem.Instance.QuestDatabase.Quests
        .OfType<NpcQuest>()
        .Where(x => x.StartTargetNpcCode == npcCode).ToList();

        takeQuests = QuestSystem.Instance.QuestDatabase.Quests
            .OfType<NpcQuest>()
            .Where(x => x.EndTargetNpcCode == npcCode).ToList();

        curState = NpcQuestState.NONE;

        curReadyQuests = new List<NpcQuest>();
    }

    public void Setting()
    {
        //현재 대기중인 퀘스트 세팅 (해당npc 퀘스트 중)
        //curReadyQuests.Clear();
        curReadyQuests = giveQuests.Where(x => 
            !QuestSystem.Instance.ContainsInActiveQuests(x) &&
            !QuestSystem.Instance.ContainsInCompleteQuests(x) &&
            x.IsAcceptable
        ).ToList();

        //현재 진행중인 퀘스트 세팅 (해당npc 퀘스트 중)
        inProgressQuest = giveQuests.FirstOrDefault(x => QuestSystem.Instance.ContainsInActiveQuests(x));

        completeReadyTakeQuest = QuestSystem.Instance.ActiveQuests.FirstOrDefault(x => IsExistTakeQuest(x) && (x.State == QuestState.WaitingForCompletion)) as NpcQuest;
        //completeReadyTakeQuest = takeQuests.FirstOrDefault(x => QuestSystem.Instance.ContainsInCompleteReadyQuests(x));
        //npcCanvas 갱신 / 상태 갱신
        npcCanvasAndStateUpdate();
    }

    private void npcCanvasAndStateUpdate()
    {
        //현재 진행중인 퀘스트가 있고 그 퀘스트가 완료대기상태일때 npc머리위에 완료 대기 이미지(?)를 보이게합니다
        if (completeReadyTakeQuest != null)
        {
            npcCanvas.SettingQuestState(NpcQuestState.QUEST_COMPLETE_READY);
            curState = NpcQuestState.QUEST_COMPLETE_READY;
        }

        //현재 진행중인 퀘스트가 완료가아닌 진행중이라면 진행중 표시(...)를 보이게 합니다.
        else if (inProgressQuest != null)
        {
            npcCanvas.SettingQuestState(NpcQuestState.QUEST_IN_PROGRESS);
            curState = NpcQuestState.QUEST_IN_PROGRESS;
        }

        //현재 진행중인 퀘스트가 없고 대기 퀘스트가 있을시 (!)표시가 뜨게 합니다.
        else if (curReadyQuests.Count > 0)
        {
            npcCanvas.SettingQuestState(NpcQuestState.QUEST_READY);
            curState = NpcQuestState.QUEST_READY;
        }

        //현재 진행중인 퀘스트가 없고 대기 퀘스트도 없을시 퀘스트를 받을 수 없는 상태이므로 npc머리위에 이미지가 뜨지 않습니다.
        else
        {
            npcCanvas.SettingQuestState(NpcQuestState.NONE);
            curState = NpcQuestState.NONE;
        }
    }

    private bool IsGiveOrTakeQuest(Quest _quest)
    {
        bool check = false;

        if (giveQuests != null)
        {
            check = giveQuests.Any(x => x.CodeName == _quest.CodeName);
        }

        if (takeQuests != null)
        {
            check |= takeQuests.Any(x => x.CodeName == _quest.CodeName);
        }

        return check;
    }

    private bool IsExistTakeQuest(Quest _quest)
    {
        return takeQuests.Any(x => x.CodeName == _quest.CodeName);
    }

    //이벤트
    private void OnQuestRegistered(Quest _quest)
    {
        if(IsGiveOrTakeQuest(_quest))
            Setting();
    }

    private void OnQuestCompletedReady(Quest _quest)
    {
        if (IsGiveOrTakeQuest(_quest))
            Setting();
    }

    private void OnQuestCompleted(Quest _quest)
    {
        //if (IsGiveOrTakeQuest(_quest))
            Setting();
    }

    private void OnQuestCanceled(Quest _quest)
    {
        if (
         ((inProgressQuest != null) && (inProgressQuest.CodeName == _quest.CodeName)) ||
         ((completeReadyTakeQuest != null) && (completeReadyTakeQuest.CodeName == _quest.CodeName))
        )
        {
            Setting();
        }
    }

    private void OnDestroy()
    {
        if (QuestSystem.Instance != null)
        {
            QuestSystem.Instance.onQuestRegistered -= OnQuestRegistered;
            QuestSystem.Instance.onQuestCompletedReady -= OnQuestCompletedReady;
            QuestSystem.Instance.onQuestCompleted -= OnQuestCompleted;
            QuestSystem.Instance.onQuestCanceled -= OnQuestCanceled;
        }
    }
}
