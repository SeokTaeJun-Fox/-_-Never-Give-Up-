using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestListWindowModel
{
    private List<NpcQuest> npcQuests;
    private Action<NpcQuest> onClickElementButton;

    public QuestListWindowModel(List<NpcQuest> _npcQuests, Action<NpcQuest> _onClickElementButton)
    {
        this.npcQuests = _npcQuests;
        onClickElementButton = _onClickElementButton;
    }

    public IReadOnlyList<NpcQuest> NpcQuests => npcQuests;
    public Action<NpcQuest> OnClickElementButton => onClickElementButton;
}
