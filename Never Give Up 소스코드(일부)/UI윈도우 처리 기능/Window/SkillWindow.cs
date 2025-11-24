using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 플레이어의 스킬 정보를 보여주는 UI 윈도우입니다.
// 스킬 포인트, 보유 스킬, 스킬 설명 패널을 초기화하고 창 열기/닫기 및 이벤트를 처리합니다.
// 의존성 주입, 의존성 역전 원칙을 적용했습니다.
public class SkillWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private SkillPointPanelController skillPointPanel; //스킬 포인트 패널
    [SerializeField] private MySkillPanelController mySkillPanel;   //보유 스킬 패널
    [SerializeField] private SkillDescPanelController skillDescPanel;   //스킬 설명 패널
    [SerializeField] private Button exitButton;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 스크립터블 인터페이스들
    private ISkillManager skillManager;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;   //창 열림/닫힘 보고 이벤트

    private void Awake()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    // 외부에서 의존성 주입을 받아 인터페이스를 설정합니다.
    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is ISkillManager skillManager)
                this.skillManager = skillManager;
        }
    }

    // 초기 데이터를 받아 설정합니다.
    public void Initial(object[] _datas)
    {
        //각 패널 요소들을 초기화 합니다.(인터페이스 주입포함)
        if(skillPointPanel != null) skillPointPanel.Initial(skillManager);
        if(mySkillPanel != null) mySkillPanel.Initial(skillManager);
        if(skillDescPanel != null) skillDescPanel.Initial(skillManager);

        // 스킬 선택 시 설명 패널 갱신 연결
        if (skillDescPanel != null && mySkillPanel != null)
        {
            mySkillPanel.OnPanelElementSelected += skillDescPanel.Setting;
        }
    }

    // 스킬 창을 엽니다.
    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //UI를 최상단으로 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);    //창 열림 이벤트 보고

        //각 패널 열기
        if (skillPointPanel != null) skillPointPanel.OpenPanel();
        if (mySkillPanel != null) mySkillPanel.OpenPanel();
        if (skillDescPanel != null) skillDescPanel.OpenPanel();

        onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        //각 패널 닫기
        if (skillPointPanel != null) skillPointPanel.ClosePanel();
        if (mySkillPanel != null) mySkillPanel.ClosePanel();
        if (skillDescPanel != null) skillDescPanel.ClosePanel();

        onClose?.Invoke();
    }

    //이벤트용
    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
