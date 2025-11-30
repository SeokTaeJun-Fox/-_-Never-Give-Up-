using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAbilityWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private PlayerAbilityView abilityView;

    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnClose;

    //이 클래스에 필요한 스크립터블 인터페이스들
    private IPlayerAbilityManager abilityManager;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    private void Awake()
    {
        abilityView.CloseButton.onClick.AddListener(OnClickExitButton);
        abilityView.UiBlockButton.onClick.AddListener(OnClickExitButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IPlayerAbilityManager am)
                abilityManager = am;
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

        if (abilityManager != null)
        {
            abilityView.Setting(abilityManager.PlayerAbilities);
        }

        OnOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnClose?.Invoke();
    }

    //이벤트용
    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
