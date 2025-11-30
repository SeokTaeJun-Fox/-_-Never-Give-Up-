using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbyImageAppearEffect : MonoBehaviour
{
    private Tween scaleTween;
    [SerializeField] private Vector2 firstScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private Ease ease;
    [SerializeField] private float animatorTime;

    [SerializeField] private UnityEvent onShow;

    private TweenCallback onComplete;
    public TweenCallback OnComplete
    {
        set => onComplete = value;
        get => onComplete;
    }

    private void Awake()
    {
        scaleTween = transform.DOScale(finalScale, animatorTime).From(firstScale).SetEase(ease);
        scaleTween.SetAutoKill(false);
        scaleTween.onComplete = onComplete;
        scaleTween.Pause();
    }

    public void Show()
    {
        scaleTween.Restart();
        onShow?.Invoke();
    }
}
