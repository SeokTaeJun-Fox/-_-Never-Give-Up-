using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbyButtonAppearEffect : MonoBehaviour
{
    private Tween scaleTween;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float firstX;
    [SerializeField] private float finalX;
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
        scaleTween = rectTransform.DOAnchorPosX(finalX, animatorTime).From(new Vector2(firstX, rectTransform.anchoredPosition.y));
        scaleTween.SetAutoKill(false);
        scaleTween.onComplete = onComplete;
        scaleTween.Pause();
    }

    public void Show()
    {
        scaleTween.Restart();
        onShow?.Invoke();
    }

    public void Initial()
    {
        scaleTween.Rewind();
    }
}
