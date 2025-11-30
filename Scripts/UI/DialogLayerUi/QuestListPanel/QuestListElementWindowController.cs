using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestListElementWindowController : MonoBehaviour
{
    [SerializeField] private QuestListElementWindowView view;
    private NpcQuest npcQuest;

    public event Action<NpcQuest> OnClickButton;

    private void Awake()
    {
        view.Button.onClick.AddListener(OnClickPanelButton);
    }

    public void Setting(NpcQuest _quest)
    { 
        npcQuest = _quest;

        if (npcQuest != null)
        {
            string content = $"[{npcQuest.Category.DisplayName}] {npcQuest.DisplayName}";
            view.SetContent(content);
        }
    }

    public void OnClickPanelButton()
    {
        OnClickButton?.Invoke(npcQuest);
    }
}
