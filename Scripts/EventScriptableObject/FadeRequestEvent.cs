using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//페이드인, 페이드아웃 요청이벤트 스크립터블 오브젝트입니다. 
[CreateAssetMenu(fileName = "FadeRequestEvent", menuName = "Scriptable Object/Event/FadeRequestEvent")]
public class FadeRequestEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<TweenCallback> onFadeInRequest;
    private Action<TweenCallback> onFadeOutRequest;

    public event Action<TweenCallback> OnFadeInRequest
    {
        add { onFadeInRequest += value; }
        remove { onFadeInRequest -= value; }
    }

    public event Action<TweenCallback> OnFadeOutRequest
    {
        add { onFadeOutRequest += value; }
        remove { onFadeOutRequest -= value; }
    }

    public void RaiseFadeIn(TweenCallback _onFadeInComplete) => onFadeInRequest?.Invoke(_onFadeInComplete);
    public void RaiseFadeOut(TweenCallback _onFadeOutComplete) => onFadeOutRequest?.Invoke(_onFadeOutComplete);

    public override void Initial()
    {
        onFadeInRequest = null;
        onFadeOutRequest = null;
    }
}
