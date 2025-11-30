using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingWindowView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressTmp;
    [SerializeField] private Image progressImage;

    [SerializeField] private RectTransform panel;

    public void SetView(float _progress)
    {
        string progressText = ((int)(_progress * 100)).ToString();
        progressTmp.text = progressText + "%";
        progressImage.fillAmount = _progress;
    }

    public void InitialPanelSize()
    { 
        panel.localScale = Vector3.one;
    }

    public void EffectAppear()
    {
        panel.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear).From(Vector3.zero);
    }

    public void EffectDisappear(TweenCallback _onComplete)
    {
        panel.DOKill();
        panel.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce).From(Vector3.one).OnComplete(_onComplete);
    }
}
