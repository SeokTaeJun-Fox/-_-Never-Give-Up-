using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestWindowDetailPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Quest target;
    public Quest Target { get => target; }

    //title
    [SerializeField] private TextMeshProUGUI title;
    
    //head
    [SerializeField] private Image npcImage;
    [SerializeField] private TextMeshProUGUI npcNameTmp;
    [SerializeField] private TextMeshProUGUI content;

    //task
    [SerializeField] private QuestWindowDetailTaskElement taskElementPreb;
    [SerializeField] private Transform taskElementPrebParent;
    private List<QuestWindowDetailTaskElement> taskElementCache = new();

    //reward
    [SerializeField] private QuestWindowDetailRewardElement rewardElementPreb;
    [SerializeField] private Transform rewardElementPrebParent;
    private List<QuestWindowDetailRewardElement> rewardElementCache = new();

    //OutView
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        cancelButton.onClick.AddListener(CancelQuest);
    }

    public void Show(Quest _quest)
    {
        //ÃÊ±âÈ­
        panel.SetActive(true);
        target = _quest;

        foreach (var taskElement in taskElementCache)
            taskElement.gameObject.SetActive(false);

        foreach (var rewardElement in rewardElementCache)
            rewardElement.gameObject.SetActive(false);

        //title
        string addCategory = _quest.Category == null ? "" : $"[{_quest.Category.DisplayName}] ";
        title.text = $"{addCategory}{_quest.DisplayName}";

        //head
        NpcQuest npcQuest = _quest as NpcQuest;
        if (npcQuest != null)
        {
            npcImage.sprite = npcQuest.StartTargetNpcSpr;
            npcNameTmp.text = npcQuest.StartTargetNpcName;
        }
        content.text = _quest.Description;

        //task
        if (_quest.TaskGroups.Count > 0)
        {
            IReadOnlyList<Task> tasks = _quest.TaskGroups[0].Tasks;
            for (int index = 0; index < tasks.Count; index++)
            {
                string taskDesc = $"{tasks[index].Description} ({tasks[index].CurrentSuccess}/{tasks[index].NeedSuccessToComplete})";

                if (index >= taskElementCache.Count)
                {
                    var taskElement = Instantiate(taskElementPreb, taskElementPrebParent);
                    taskElement.Setting(taskDesc, tasks[index].CurrentSuccess >= tasks[index].NeedSuccessToComplete);
                    taskElementCache.Add(taskElement);
                }
                else
                {
                    taskElementCache[index].gameObject.SetActive(true);
                    taskElementCache[index].Setting(taskDesc, tasks[index].CurrentSuccess >= tasks[index].NeedSuccessToComplete);
                }
            }
        }

        //reward
        IReadOnlyList<Reward> rewards = _quest.Rewards;
        for (int index = 0; index < rewards.Count; index++)
        {
            string content = $"{rewards[index].Description} *{rewards[index].Quantity}";

            if (index >= rewardElementCache.Count)
            {
                var rewardElement = Instantiate(rewardElementPreb, rewardElementPrebParent);
                rewardElement.Setting(rewards[index].Icon, content);
                rewardElementCache.Add(rewardElement);
            }
            else
            {
                rewardElementCache[index].gameObject.SetActive(true);
                rewardElementCache[index].Setting(rewards[index].Icon, content);
            }
        }

        //outButton
        cancelButton.gameObject.SetActive(_quest.IsCancelable && _quest.IsRegistered && !_quest.IsComplete);
    }

    private void CancelQuest()
    {
        if (Target.IsCancelable)
            Target.Cancel();
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
        target = null;
        cancelButton.gameObject.SetActive(false);
    }
}
