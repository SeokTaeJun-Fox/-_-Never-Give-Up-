using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupWindowManager : MonoBehaviour
{
    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;
    [SerializeField] private RequestOpenPopupWindowEvent openPopupWindowEvent;

    private void Awake()
    {
        openPopupWindowEvent.OnRequest += OpenPopup;
    }

    public void OpenPopup(string _title, string _content, bool _isTwoButtonActive, string _yesButtonText, UnityAction _onClickYesButton, string _noButtonText = "", UnityAction _onClickNoButton = null)
    {
        PopupWindowModel model = new PopupWindowModel(_title, _content, _isTwoButtonActive, _yesButtonText, _onClickYesButton, _noButtonText, _onClickNoButton);
        factoryEvent.Raise(UIType.POPUP_WINDOW, new object[] { model });
    }

    private void OnDestroy()
    {
        openPopupWindowEvent.OnRequest -= OpenPopup;
    }
}
