using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private QuestListElementWindowController preb;
    [SerializeField] private Transform prebParent;

    private QuestListWindowModel model;
    private List<QuestListElementWindowController> cache;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;
    [SerializeField] private UnityEvent clickElementButtonEvent;

    private bool isOpenFirst = true;

    private void Awake()
    {
        cache = new List<QuestListElementWindowController>();
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {

        }
    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is QuestListWindowModel _model)
                model = _model;
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        Setting();
        onOpen?.Invoke();
    }

    private void Setting()
    {
        if (model == null)
            return;

        int index = 0;

        foreach (var quest in model.NpcQuests)
        {
            if (!quest.IsRegistered)
            {
                if (index < cache.Count)
                {
                    cache[index].gameObject.SetActive(true);
                    cache[index].Setting(quest);
                }
                else
                { 
                    var newElement = Instantiate(preb, prebParent);
                    newElement.Setting(quest);
                    newElement.OnClickButton += OnClickElementButton;
                    cache.Add(newElement);
                }

                index++;
            }
        }
    }

    public void Close()
    {
        foreach (var element in cache)
            element.gameObject.SetActive(false);

        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트용
    private void OnClickElementButton(NpcQuest _npcQuest)
    {
        //if (_npcQuest == null || _npcQuest.IsRegistered)
        //    return;

        //QuestSystem.Instance.Register(_npcQuest);   //서비스 로케이터로 이동시 수정예정
        clickElementButtonEvent?.Invoke();
        openEvent?.RaiseCloseWindow();
        Close();

        if (model != null)
        {
            model.OnClickElementButton?.Invoke(_npcQuest);
        }
    }

    public void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
