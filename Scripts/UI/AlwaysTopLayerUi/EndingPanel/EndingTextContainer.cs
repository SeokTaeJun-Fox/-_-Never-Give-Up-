using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BitWave_Labs.AnimatedTextReveal;
using System;

public class EndingTextContainer : MonoBehaviour
{
    [SerializeField] private AnimatedTextReveal[] AnimatedTmps;
    private int completeTmp;
    private int needCompleteTmp;

    public event Action OnComplete;

    private void Awake()
    {
        needCompleteTmp = AnimatedTmps.Length;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        completeTmp = 0;

        foreach (var animatedTmp in AnimatedTmps)
            StartCoroutine(CoShowText(animatedTmp));
    }

    private IEnumerator CoShowText(AnimatedTextReveal _animatedTmp)
    {
        _animatedTmp.TextMesh.alignment = TMPro.TextAlignmentOptions.TopLeft;
        _animatedTmp.SetAllCharactersAlpha(0);

        yield return StartCoroutine(_animatedTmp.FadeText(true));
        
        completeTmp++;
        if (needCompleteTmp == completeTmp)
            OnComplete?.Invoke();
    }
}
