using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

//대화창의 흐름을 제어하는 컨트롤러 클래스입니다.
//모델(DialogPanelModel)과 뷰(DialogPanelView)를 연결하며,
//사용자 입력에 따라 대화 진행, 버튼 패널 표시, 퀘스트 이벤트 등을 처리합니다.
public class DialogPanelController : MonoBehaviour, IUIWindow
{
    [SerializeField] private DialogPanelView view;
    private DialogPanelModel model;

    [SerializeField] private UnityEvent onOpen;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    private bool isOpen;

    private void Awake()
    {
        view.ContentPanelButton.onClick.AddListener(OnClickContentPanelButton);
        view.QuestButton.onClick.AddListener(OnClickQuestButton);
    }

    //외부 의존성 주입을 위한 메소드입니다.
    public void InjectDependencies(object[] _dependencies)
    {

    }

    //컨트롤러 초기화, 모델 데이터를 받아 뷰를 설정합니다.
    //창이 열린 상태일 경우 첫 대사를 표시합니다.
    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is DialogPanelModel _model)
                model = _model;
        }

        view.ShowButtonPanel(false);

        //열린상태에서 다이얼로그 갱신시 실행
        if (isOpen)
        {
            if (model != null)
            {
                model.OnLineChanged += OnLineChanged;
                model.SetLine(0);
            }
        }
    }

    //대화창을 열고 첫 대사를 표시합니다.
    public void Open()
    {
        if (!isOpen)
            onOpen?.Invoke();

        isOpen = true;

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (model != null)
        {
            model.OnLineChanged += OnLineChanged;
            model.SetLine(0);
        }
    }

    public void Close()
    {
        isOpen = false;

        if (model != null)
        {
            model.OnLineChanged -= OnLineChanged;
        }

        view.InitialView();
        gameObject.SetActive(false);

        if(model != null)
            model.OnClose?.Invoke();
    }

    //이벤트
    //모델에서 대사 변경 이벤트가 발생했을 때 호출됩니다.
    //뷰에 대사를 표시하고 버튼 패널 표시 여부를 결정합니다.
    private void OnLineChanged(DialogueLine _dialogueLine)
    {
        view.ShowDialog(_dialogueLine, model.IsEndLine() && model.IsButtonPanelOpen);
    }

    //대화창 클릭 시 호출됩니다.
    //타이핑 중이면 스킵하고, 완료되었으면 다음 대사로 진행하거나 종료합니다.
    private void OnClickContentPanelButton()
    {
        if (model != null)
        {
            if (!view.IsTypeEnd)
            {
                view.SkipTyping();

                if (model.IsEndLine() && model.IsButtonPanelOpen)
                    view.ShowButtonPanel(true);
            }
            else
            {
                if (!model.IsEndLine())
                    model.NextLine();
                else
                {
                    openEvent?.RaiseCloseWindow();
                    Close();
                }
            }
        }
    }

    //퀘스트 버튼 클릭 시 호출됩니다.
    //모델에 등록된 퀘스트 이벤트를 실행합니다.
    private void OnClickQuestButton()
    {
        if (model != null)
            model.OnClickQuestButton?.Invoke();
    }
}
