using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImageAnimator : MonoBehaviour
{
    private Tween scaleTween;
    [SerializeField] private Vector2 firstScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private float animatorTime;

    [SerializeField] private UnityEvent OnShow;

    private TweenCallback onComplete;
    public TweenCallback OnComplete
    {
        set => onComplete = value;
        get => onComplete;
    }

    private void Awake()
    {
        scaleTween = transform.DOScale(finalScale, animatorTime).From(firstScale);
        scaleTween.SetAutoKill(false);
        scaleTween.onComplete = onComplete;
        scaleTween.Pause();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        scaleTween.Restart();
        OnShow?.Invoke();
    }
}
