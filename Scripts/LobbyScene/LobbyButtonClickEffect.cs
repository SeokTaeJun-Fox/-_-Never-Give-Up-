using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LobbyButtonClickEffect : MonoBehaviour
{
    private Tween scaleTween;
    [SerializeField] private Vector2 firstScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private Ease ease;
    [SerializeField] private float animatorTime;

    public event Action OnComplete;

    private void Awake()
    {
        OnComplete += Initial;

        scaleTween = transform.DOScale(finalScale, animatorTime).From(firstScale).SetEase(ease);
        scaleTween.SetAutoKill(false);
        scaleTween.onComplete = InvokeOnComplete;
        scaleTween.Pause();
    }

    public void Show()
    {
        scaleTween.Restart();
    }

    public void Initial()
    {
        scaleTween.Rewind();
    }

    private void InvokeOnComplete()
    {
        OnComplete?.Invoke();
    }
}
