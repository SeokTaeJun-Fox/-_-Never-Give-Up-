using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadePreb;
    private Image fadeImg;

    //이벤트 스크립터블 오브젝트
    [SerializeField] private GetCanvasRequestEvent canvasRequestEvent;
    [SerializeField] private FadeRequestEvent fadeRequestEvent;

    private void Awake()
    {
        fadeRequestEvent.OnFadeInRequest += FadeIn;
        fadeRequestEvent.OnFadeOutRequest += FadeOut;
    }

    public void FadeIn(TweenCallback _onFadeInComplete)
    {
        if (fadeImg == null)
        {
            GameObject parent = null;
            if (canvasRequestEvent != null)
                parent = canvasRequestEvent.RaiseGet(CanvasType.AlwaysTopLayer);

            fadeImg = Instantiate(fadePreb, parent.transform);
        }

        fadeImg.enabled = true;
        fadeImg.DOFade(1, 0.5f).OnComplete(_onFadeInComplete);
    }

    public void FadeOut(TweenCallback _onFadeOutComplete)
    {
        _onFadeOutComplete += () => { fadeImg.enabled = false; };

        if (fadeImg == null)
        {
            GameObject parent = null;
            if (canvasRequestEvent != null)
                parent = canvasRequestEvent.RaiseGet(CanvasType.AlwaysTopLayer);

            fadeImg = Instantiate(fadePreb, parent.transform);
        }

        fadeImg.DOFade(0, 0.5f).OnComplete(_onFadeOutComplete);
    }

    private void OnDestroy()
    {
        fadeRequestEvent.OnFadeInRequest -= FadeIn;
        fadeRequestEvent.OnFadeOutRequest -= FadeOut;
    }
}
