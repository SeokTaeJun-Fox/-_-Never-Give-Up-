using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PopupWindowModel
{
    public string title;
    public string content;
    public bool isTwoButtonActive;
    public string yesButtonText;
    public string noButtonText;

    public UnityAction OnClickYesButton;
    public UnityAction OnClickNoButton;

    public PopupWindowModel(string _title, string _content, bool _isTwoButtonActive, string _yesButtonText, UnityAction _onClickYesButton, string _noButtonText = "", UnityAction _onClickNoButton = null)
    { 
        title = _title;
        content = _content;
        isTwoButtonActive = _isTwoButtonActive;
        yesButtonText = _yesButtonText;
        noButtonText = _noButtonText;

        OnClickYesButton = _onClickYesButton;
        OnClickNoButton = _onClickNoButton;
    }
}
