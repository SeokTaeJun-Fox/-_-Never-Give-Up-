using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoticePanelController : MonoBehaviour
{
    [SerializeField] private RectTransform initPos;
    [SerializeField] private float initialYPos;
    [SerializeField] private float showYPos;
    [SerializeField] private float showTime;
    private Tweener showTween;
    private Tweener hideTween;
    private Queue<string> noticeQueue;
    Coroutine coNotice;
    bool isPlayingShowNotice;
    bool isStop;

    [SerializeField] private NoticePanelView view;

    [SerializeField] private UnityEvent onNotice;

    public static NoticePanelController Instance
    {
        get => instance;
    }
    private static NoticePanelController instance;

    //이 클래스에 필요한 인터페이스들
    private ISceneLoader sceneLoader;

    private void Start()
    {
        showTween = view.Panel.DOAnchorPosY(showYPos, 0.5f)
          .SetAutoKill(false)
          .SetEase(Ease.Linear)
          .From(new Vector2(0, initialYPos))
          .Pause();

        hideTween = view.Panel.DOAnchorPosY(initialYPos, 0.5f)
                  .SetAutoKill(false)
                  .SetEase(Ease.Linear)
                  .From(new Vector2(0, showYPos))
                  .Pause();

        view.Panel.anchoredPosition = initPos.anchoredPosition;

        noticeQueue = new Queue<string>();

        var questSystem = QuestSystem.Instance;
        questSystem.onQuestRegistered += OnQuestRegistered;
        questSystem.onQuestCompletedReady += OnQuestCompletedReady;

        sceneLoader = ServiceLocator.GetService<ISceneLoader>();

        if (sceneLoader != null)
        {
            sceneLoader.OnSceneLoaded += StopNotice;
            sceneLoader.OnLoadComplete += ResumeNotice;
        }
    }

    public void StopNotice()
    {
        view.Panel.anchoredPosition = initPos.anchoredPosition;
        showTween.Pause();
        hideTween.Pause();

        if (coNotice != null)
            StopCoroutine(coNotice);
    }

    public void ResumeNotice()
    {
        if (noticeQueue.Count >= 0)
            coNotice = StartCoroutine(CoShowNotice());
    }

    private IEnumerator CoShowNotice()
    {
        isPlayingShowNotice = true;

        while (noticeQueue.Count > 0)
        {
            onNotice?.Invoke();

            showTween.Restart();
            string noticeTxt = noticeQueue.Dequeue();
            view.ContentTmp.text = noticeTxt;

            yield return new WaitForSeconds(showTime + 0.5f);

            hideTween.Restart();

            yield return new WaitForSeconds(0.5f);
        }

        isPlayingShowNotice = false;
    }

    public void Active(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    //이벤트
    public void OnQuestRegistered(Quest _quest)
    {
        if (!GlobalData.isGameStart)
            return;

        noticeQueue.Enqueue($"\"{_quest.DisplayName}\" 퀘스트 시작");

        if (!isPlayingShowNotice)
            coNotice = StartCoroutine(CoShowNotice());
    }

    public void OnQuestCompletedReady(Quest _quest)
    {
        if (!GlobalData.isGameStart)
            return;

        noticeQueue.Enqueue($"\"{_quest.DisplayName}\" 퀘스트 완료 대기");

        if (!isPlayingShowNotice)
            coNotice = StartCoroutine(CoShowNotice());
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;
        if (questSystem != null)
        {
            questSystem.onQuestRegistered -= OnQuestRegistered;
            questSystem.onQuestCompletedReady -= OnQuestCompletedReady;
        }

        if (sceneLoader != null)
        {
            sceneLoader.OnSceneLoaded -= StopNotice;
            sceneLoader.OnLoadComplete -= ResumeNotice;
        }
    }
}
