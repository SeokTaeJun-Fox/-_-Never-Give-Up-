using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPanelModel
{
    public Action OnComplete;

    public EndingPanelModel(Action _onComplete)
    { 
        OnComplete = _onComplete;
    }
}
