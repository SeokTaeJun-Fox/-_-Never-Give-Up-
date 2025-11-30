using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//팝업 윈도우 오픈 요청 이벤트 스크립터블 오브젝트 입니다.
[CreateAssetMenu(fileName = "RequestOpenPopupWindowEvent", menuName = "Scriptable Object/Event/RequestOpenPopupWindowEvent")]
public class RequestOpenPopupWindowEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<string, string, bool, string, UnityAction, string, UnityAction> onRequest;

    public event Action<string, string, bool, string, UnityAction, string, UnityAction> OnRequest
    {
        add { onRequest += value; }
        remove { onRequest -= value; }
    }

    public void Raise(string _title, string _content, bool _isTwoButtonActive, string _yesButtonText, UnityAction _onClickYesButton, string _noButtonText = "", UnityAction _onClickNoButton = null) 
        => onRequest?.Invoke(_title, _content, _isTwoButtonActive, _yesButtonText, _onClickYesButton, _noButtonText, _onClickNoButton);

    public override void Initial()
    {
        onRequest = null;
    }
}
