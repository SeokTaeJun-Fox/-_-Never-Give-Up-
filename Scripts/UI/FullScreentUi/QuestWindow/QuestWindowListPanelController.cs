using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestWindowListPanelController : MonoBehaviour
{
    [SerializeField] private ToggleGroup tabGroup;
    [SerializeField] private Toggle firstToggle;
    [SerializeField] private QuestWindowListPanel ReadyQuestListPanel;
    [SerializeField] private QuestWindowListPanel activeQuestListPanel;
    [SerializeField] private QuestWindowListPanel completedQuestListPanel;

    public IEnumerable<Toggle> Tabs => tabGroup.ActiveToggles();

    public Toggle FirstToggle { get => firstToggle; }

    //대기 퀘스트패널
    public void AddQuestToReadyListView(Quest quest, UnityAction<bool> onClicked)
    => ReadyQuestListPanel.AddElement(quest, onClicked);

    public void RemoveQuestFromReadyListView(Quest quest)
        => ReadyQuestListPanel.RemoveElement(quest);

    //해당 퀘스트패널이 활성화되었는지 확인합니다.
    public bool CheckActiveQuestPanelFromReadyListView(Quest _quest)
        => ReadyQuestListPanel.CheckElement(_quest);

    //진행중 퀘스트패널
    public void AddQuestToActiveListView(Quest quest, UnityAction<bool> onClicked)
        => activeQuestListPanel.AddElement(quest, onClicked);

    public void RemoveQuestFromActiveListView(Quest quest)
        => activeQuestListPanel.RemoveElement(quest);

    //완료 퀘스트패널
    public void AddQuestToCompletedListView(Quest quest, UnityAction<bool> onClicked)
        => completedQuestListPanel.AddElement(quest, onClicked);

    //모든 퀘스트패널
    public void InitialListView()
    {
        ReadyQuestListPanel.RemoveAllElement();
        activeQuestListPanel.RemoveAllElement();
        completedQuestListPanel.RemoveAllElement();
    }
}
