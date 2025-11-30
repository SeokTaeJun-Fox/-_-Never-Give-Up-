using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnClose;
    [SerializeField] private MenuWindowView view;
    [SerializeField] private int buildLobbySceneNum;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;
    [SerializeField] private RequestOpenPopupWindowEvent popupRequest;
    [SerializeField] private LoadSceneRequestEvent loadSceneRequest;
    [SerializeField] private ActiveMainUIRequestEvent activeMainUIRequest;
    [SerializeField] private ShowUIWindowFactoryEvent uiFactoryRequest;

    [SerializeField] private DataInitialRequestEvent dataInitialRequest;

    //이 클래스에 필요한 인터페이스들
    private ISaveLoadManager saveLoadManager;

    private void Awake()
    {
        view.SaveButton.onClick.AddListener(Save);
        view.SettingButton.onClick.AddListener(OpenSettingWindow);
        view.GoLobbyButton.onClick.AddListener(GoToLobby);
        view.CloseButton.onClick.AddListener(OnClickExitButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is ISaveLoadManager slm)
                saveLoadManager = slm;
        }
    }

    public void Initial(object[] _datas)
    {

    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        OnOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnClose?.Invoke();
    }

    //이벤트
    private void Save()
    {
        if (saveLoadManager != null)
        {
            saveLoadManager.Save();
            if (popupRequest != null)
            {
                string title = "저장";
                string content = "저장을 완료했습니다.";
                string yes = "확인";

                popupRequest.Raise(title, content, false, yes, null);
            }
        }
    }

    private void OpenSettingWindow()
    {
        uiFactoryRequest.Raise(UIType.SETTING_WINDOW);
    }

    private void GoToLobby()
    {
        if (loadSceneRequest != null)
        {
            openEvent?.RaiseCloseWindow();
            Close();

            if(dataInitialRequest != null)
                dataInitialRequest.Raise();

            SceneManager.LoadScene(buildLobbySceneNum);
        }
    }

    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
