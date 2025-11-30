using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PopupWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private PopupWindowView view;
    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    private PopupWindowModel model;

    private void Awake()
    {
        view.YesButton.onClick.AddListener(OnClickYesButton);
        view.NoButton.onClick.AddListener(OnClickNoButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is PopupWindowModel popupWindowModel)
                model = popupWindowModel;
        }

        Setting();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        onOpen?.Invoke();
    }

    private void Setting()
    {
        if (model == null)
            return;

        view.SetTitle(model.title);
        view.SetContent(model.content);
        view.ActiveButton(model.isTwoButtonActive);
        
        view.SetYesButtonText(model.yesButtonText);

        if (model.isTwoButtonActive)
        {
            view.SetNoButtonText(model.noButtonText);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트용

    private void OnClickYesButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();

        model.OnClickYesButton?.Invoke();
    }

    private void OnClickNoButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();

        model.OnClickNoButton?.Invoke();
    }

    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
