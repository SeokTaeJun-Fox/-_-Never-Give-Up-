using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject uiBlock; 

    [SerializeField] private LobbyImageAppearEffect LogoImageEffect;
    [SerializeField] private LobbyButtonAppearEffect[] buttonAppearEffects;

    [SerializeField] private LobbyButtonClickEffect NewGameButtonClickEffect;
    [SerializeField] private LobbyButtonClickEffect LoadGameButtonClickEffect;
    [SerializeField] private LobbyButtonClickEffect SettingButtonClickEffect;
    [SerializeField] private LobbyButtonClickEffect ExitButtonClickEffect;

    [SerializeField] private Button LoadGameButton;
    [SerializeField] private Button DeleteDataButton;

    //이벤트 스크립터블 오브젝트
    [SerializeField] private ShowUIWindowFactoryEvent uiFactoryEvent;
    [SerializeField] private LoadSceneRequestEvent loadSceneRequestEvent;
    [SerializeField] private RequestOpenPopupWindowEvent popupRequest;
    [SerializeField] private ActiveMainUIRequestEvent activeMainUIRequestEvent;

    //기타
    [SerializeField] private UnityEvent OnNewGame;

    ISaveLoadManager saveLoadManager;

    private void Awake()
    {
        NewGameButtonClickEffect.OnComplete += UIBlockOff;
        NewGameButtonClickEffect.OnComplete += NewGame;

        LoadGameButtonClickEffect.OnComplete += UIBlockOff;
        LoadGameButtonClickEffect.OnComplete += LoadGame;

        SettingButtonClickEffect.OnComplete += UIBlockOff;
        SettingButtonClickEffect.OnComplete += ShowSettingUI;

        ExitButtonClickEffect.OnComplete += UIBlockOff;
        ExitButtonClickEffect.OnComplete += ExitGame;

        DeleteDataButton.gameObject.SetActive(false);
        DeleteDataButton.onClick.AddListener(DeleteData);

        saveLoadManager = ServiceLocator.GetService<ISaveLoadManager>();
        UpdateButtonAboutData();

        activeMainUIRequestEvent.Raise(false);

        GlobalData.isGameStart = false;
    }

    private void Start()
    {
        StartCoroutine(CoAppearAllUI());
    }

    private IEnumerator CoAppearAllUI()
    {
        uiBlock.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        LogoImageEffect.Show();

        yield return new WaitForSeconds(0.7f);

        foreach (var effect in buttonAppearEffects)
        {
            effect.Show();
            yield return new WaitForSeconds(0.2f);
        }

        uiBlock.SetActive(false);
        DeleteDataButton.gameObject.SetActive(true);
    }

    public void UIBlockOn()
    { 
        uiBlock.SetActive(true);
    }

    public void UIBlockOff()
    {
        uiBlock.SetActive(false);
    }

    public void NewGame()
    {
        OnNewGame?.Invoke();
        loadSceneRequestEvent.Raise(LoadSceneType.VILLAGE_SCENE, 1);
    }

    public void LoadGame()
    {
        if (saveLoadManager != null)
            saveLoadManager.Load();
        else
            Debug.LogWarning("SaveManager가 존재하지않으므로 데이터는 로드되지 않습니다.");

        //loadSceneRequestEvent.Raise(LoadSceneType.VILLAGE_SCENE, 1);
        loadSceneRequestEvent.Raise(GlobalData.curScene, GlobalData.curMapCode);
    }

    public void ShowSettingUI()
    {
        uiFactoryEvent.Raise(UIType.SETTING_WINDOW);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PopupAboutDeleteData()
    {
        string title = "데이터 삭제";
        string content = "데이터를 삭제하시겠습니까?";
        string yes = "예";
        string no = "아니오";
        popupRequest.Raise(title, content, true, yes, DeleteData, no);
    }

    public void DeleteData()
    { 
        if (saveLoadManager != null)
        {
            saveLoadManager.Delete();
        }

        UpdateButtonAboutData();
    }

    private void UpdateButtonAboutData()
    {
        if (saveLoadManager != null)
        {
            DeleteDataButton.interactable = saveLoadManager.IsFileExist();
            LoadGameButton.interactable = saveLoadManager.IsFileExist();
        }
    }
}
