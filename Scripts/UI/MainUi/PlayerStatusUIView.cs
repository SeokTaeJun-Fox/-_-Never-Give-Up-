using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatusUIView : MonoBehaviour
{
    [SerializeField] private Image expStatusImage;
    [SerializeField] private TextMeshProUGUI levelTmp;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpTmp;
    [SerializeField] private Slider mpSlider;
    [SerializeField] private TextMeshProUGUI mpTmp;

    [SerializeField] private Transform hpBar;
    private Tweener hpTween;

    public void SetExpImgFilled(float _amount)
    {
        expStatusImage.fillAmount = _amount;
    }

    public void SetLevelText(string _text)
    { 
        levelTmp.text = _text;
    }

    public void SetHpSlider(float _amount)
    { 
        hpSlider.value = _amount;
    }

    public void SetHpText(string _text)
    { 
        hpTmp.text = _text;
    }

    public void SetMpSlider(float _amount)
    { 
        mpSlider.value = _amount;
    }

    public void SetMpText(string _text)
    { 
        mpTmp.text = _text;
    }

    private void InitShakeTween()
    {
        //hp바 흔들림 연출입니다.
        //Tween을 미리 생성하고 재사용 가능하게 설정합니다.
        hpTween = hpBar.DOShakePosition(1f, 10f)
                          .SetAutoKill(false)
                          .SetEase(Ease.OutQuad)
                          .From()
                          .Pause();
    }

    public void ShakeHpBar()
    {
        if (hpTween.IsActive())
        {
            hpTween.Restart();
        }
        else
        {
            InitShakeTween();
            hpTween.Restart();
        }
    }

}
