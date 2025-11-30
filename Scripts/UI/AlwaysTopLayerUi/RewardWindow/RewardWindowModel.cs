using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RewardWindowModel
{
    private List<Reward> rewards;
    private int curIndex;
    private Action onClose;

    public IReadOnlyList<Reward> Rewards => rewards;
    public Action OnClose => onClose;

    public int CurIndex
    {
        get => curIndex;
        set => curIndex = value;
    }

    public RewardWindowModel(List<Reward> _rewards, Action _onClose)
    { 
        rewards = _rewards;
        onClose = _onClose;
        curIndex = 0;
    }
}
