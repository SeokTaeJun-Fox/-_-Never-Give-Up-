using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RewardWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private DragCheck scrollDragCheck;
    [SerializeField] private RewardWindowMainView mainView;
    [SerializeField] private Transform rewardElementParent;
    [SerializeField] private RewardElementPanel preb;

    [SerializeField] private float scrollSpeed;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;
    [SerializeField] private UnityEvent onShowRewardElement;

    private RewardWindowModel model;
    private Coroutine showReward;
    private List<RewardElementPanel> cache = new();
    private int curCacheIndex;

    private bool isShowing;
    private bool isAlwaysScrollDown;

    private void Awake()
    {
        mainView.SkipButton.onClick.AddListener(OnClickSkipButton);
        mainView.CheckButton.onClick.AddListener(OnClickCheckButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is RewardWindowModel rewardWindowModel)
                model = rewardWindowModel;
        }
    }

    private void Update()
    {
        if (isAlwaysScrollDown && !isShowing && scrollDragCheck.IsDragging)
            isAlwaysScrollDown = false;

        if(isAlwaysScrollDown)
            mainView.ScrollRect.verticalNormalizedPosition = Mathf.Lerp(mainView.ScrollRect.verticalNormalizedPosition, 0, Time.deltaTime * scrollSpeed);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        showReward = StartCoroutine(CoShowReward());
        onOpen?.Invoke();
    }

    public void Close()
    {
        curCacheIndex = 0;
        isAlwaysScrollDown = false;
        isShowing = false;
        foreach(var element in cache)
            element.gameObject.SetActive(false);

        gameObject.SetActive(false);

        if (model != null)
            model.OnClose?.Invoke();

        onClose?.Invoke();
    }

    private IEnumerator CoShowReward()
    {
        mainView.ActiveButton(true);
        curCacheIndex = 0;
        model.CurIndex = 0;
        isAlwaysScrollDown = true;
        isShowing = true;

        yield return new WaitForSeconds(0.3f);

        if (model != null)
        {
            while (model.CurIndex < model.Rewards.Count)
            {
                if (curCacheIndex < cache.Count)
                {
                    cache[curCacheIndex].gameObject.SetActive(true);
                    cache[curCacheIndex].Setting(model.Rewards[model.CurIndex]);
                }
                else
                {
                    RewardElementPanel elementPanel = Instantiate(preb, rewardElementParent);
                    elementPanel.Setting(model.Rewards[model.CurIndex]);
                    cache.Add(elementPanel);
                }

                onShowRewardElement?.Invoke();

                model.CurIndex++;
                curCacheIndex++;

                if (model.CurIndex != model.Rewards.Count)
                    yield return new WaitForSeconds(0.15f);
            }
        }

        mainView.ActiveButton(false);
        isShowing = false;
    }

    //이벤트
    private void OnClickCheckButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }

    private void OnClickSkipButton()
    {
        if (showReward != null)
            StopCoroutine(showReward);

        if (model != null)
        {
            while (model.CurIndex < model.Rewards.Count)
            {
                if (curCacheIndex < cache.Count)
                {
                    cache[curCacheIndex].gameObject.SetActive(true);
                    cache[curCacheIndex].Setting(model.Rewards[model.CurIndex]);
                }
                else
                {
                    RewardElementPanel elementPanel = Instantiate(preb, rewardElementParent);
                    elementPanel.Setting(model.Rewards[model.CurIndex]);
                    cache.Add(elementPanel);
                }

                model.CurIndex++;
                curCacheIndex++;
            }
        }

        mainView.ActiveButton(false);
        isShowing = false;
    }
}
