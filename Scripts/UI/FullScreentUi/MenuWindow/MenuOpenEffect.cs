using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpenEffect : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Vector3 firstScale;
    [SerializeField] private Vector3 finalScale;
    [SerializeField] private float animatorTime;

    private Tween scaleTween;

    private void Awake()
    {
        scaleTween = panel.DOScale(finalScale, animatorTime).From(firstScale);
        scaleTween.SetAutoKill(false);
        scaleTween.Pause();
    }

    public void Effect()
    {
        scaleTween.Restart();
    }
}
