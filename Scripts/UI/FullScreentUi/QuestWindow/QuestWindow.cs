using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private QuestWindowListPanelController questWindowListPanelController;
    [SerializeField] private QuestWindowDetailPanel detailPanel;

    [SerializeField] private UnityEvent onQuestElementButtonClicked;
    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;
    QuestSystem questSystem;

    private bool needToInitial;

    private void Awake()
    {
        //SetListView();
        needToInitial = true;

        questSystem = QuestSystem.Instance;
        questSystem.onQuestInitial += InitialListView;
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        if (needToInitial)
        {
            SetListView();
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        questWindowListPanelController.FirstToggle.isOn = true;

        UpdateReadyPanel();
        onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    private void ShowDetail(Quest quest)
    {
        detailPanel.Show(quest);
    }

    private void HideDetail()
    {
        detailPanel.Hide();
    }

    private void UpdateReadyPanel(Quest _quest = null)
    {
        foreach (var quest in questSystem.GetReadyQuests())
        {
            if (quest.IsAcceptable && !questWindowListPanelController.CheckActiveQuestPanelFromReadyListView(quest))
                AddQuestToReadyListView(quest);

            if (!quest.IsAcceptable && questWindowListPanelController.CheckActiveQuestPanelFromReadyListView(quest))
                RemoveQuestToReadyListView(quest);
        }
    }

    private void RemoveQuestToReadyListView(Quest _quest)
    {
        questWindowListPanelController.RemoveQuestFromReadyListView(_quest);
        if(detailPanel.Target != null && detailPanel.Target.CodeName == _quest.CodeName)
            detailPanel.Hide();
    }

    private void RemoveQuestToActiveListView(Quest _quest)
    {
        questWindowListPanelController.RemoveQuestFromActiveListView(_quest);
        if (detailPanel.Target != null && detailPanel.Target.CodeName == _quest.CodeName)
            detailPanel.Hide();
    }

    private void AddQuestToReadyListView(Quest quest)
    => questWindowListPanelController.AddQuestToReadyListView(quest, isOn =>
    {
        if (isOn)
            ShowDetail(quest);
        else
            HideDetail();
    });

    //취소된 퀘스트는 파괴됩니다. 이로인해AddQuestToActiveListView로 취소된 퀘스트를 불러오고 다른변수에 저장시
    //missing reference가뜨면서 오류가 발생됩니다. 이때 AddQuestToActiveListView메소드 실행은
    //이 메소드 실행으로 대체됩니다.
    private void AddQuestToReadyListView2(Quest quest)
    {
        var questSystem = QuestSystem.Instance;
        if (questSystem == null)
            return;

        var loadQuest = questSystem.QuestDatabase.Quests.FirstOrDefault(x => x.CodeName == quest.CodeName);

        questWindowListPanelController.AddQuestToReadyListView(loadQuest, isOn =>
        {
            onQuestElementButtonClicked?.Invoke();

            if (isOn)
                ShowDetail(loadQuest);
            else
                HideDetail();
        });
    }

    private void AddQuestToActiveListView(Quest quest)
    => questWindowListPanelController.AddQuestToActiveListView(quest, isOn =>
    {
        onQuestElementButtonClicked?.Invoke();

        if (isOn)
            ShowDetail(quest);
        else
            HideDetail();
    });

    private void AddQuestToCompletedListView(Quest quest)
        => questWindowListPanelController.AddQuestToCompletedListView(quest, isOn =>
        {
            onQuestElementButtonClicked?.Invoke();

            if (isOn)
                ShowDetail(quest);
            else
                HideDetail();
        });

    private void HideDetailIfQuestCanceled(Quest _quest)
    {
        if (detailPanel.Target != null && detailPanel.Target.CodeName == _quest.CodeName)
            detailPanel.Hide();
    }

    private void InitialListView()
    {
        questWindowListPanelController.InitialListView();
        questSystem.onQuestRegistered -= AddQuestToActiveListView;
        questSystem.onQuestRegistered -= RemoveQuestToReadyListView;
        questSystem.onQuestCompleted -= RemoveQuestToActiveListView;
        questSystem.onQuestCompleted -= AddQuestToCompletedListView;
        questSystem.onQuestCompleted -= UpdateReadyPanel;

        questSystem.onQuestCanceled -= HideDetailIfQuestCanceled;
        questSystem.onQuestCanceled -= RemoveQuestToActiveListView;
        questSystem.onQuestCanceled -= AddQuestToReadyListView2;

        needToInitial = true;
    }

    private void SetListView()
    {
        //questSystem = QuestSystem.Instance;

        foreach (var quest in questSystem.GetReadyQuests())
        {
            if (quest.IsAcceptable)
                AddQuestToReadyListView(quest);
        }

        foreach (var quest in questSystem.ActiveQuests)
            AddQuestToActiveListView(quest);

        foreach (var quest in questSystem.CompletedQuests)
            AddQuestToCompletedListView(quest);

        questSystem.onQuestRegistered += AddQuestToActiveListView;
        questSystem.onQuestRegistered += RemoveQuestToReadyListView;
        questSystem.onQuestCompleted += RemoveQuestToActiveListView;
        questSystem.onQuestCompleted += AddQuestToCompletedListView;
        questSystem.onQuestCompleted += UpdateReadyPanel;
        //questSystem.onQuestCompleted += HideDetailIfQuestCanceled;
        questSystem.onQuestCanceled += HideDetailIfQuestCanceled;
        questSystem.onQuestCanceled += RemoveQuestToActiveListView;
        questSystem.onQuestCanceled += AddQuestToReadyListView2;

        needToInitial = false;
    }

    public void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;
        if (questSystem)
        {
            questSystem.onQuestRegistered -= AddQuestToActiveListView;
            questSystem.onQuestRegistered -= RemoveQuestToReadyListView;
            questSystem.onQuestCompleted -= RemoveQuestToActiveListView;
            questSystem.onQuestCompleted -= AddQuestToCompletedListView;

            questSystem.onQuestCanceled -= HideDetailIfQuestCanceled;
            questSystem.onQuestCanceled -= RemoveQuestToActiveListView;
            questSystem.onQuestCanceled -= AddQuestToReadyListView2;

            questSystem.onQuestInitial -= InitialListView;
        }
    }
}
