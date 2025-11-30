using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//단일 버프 UI 요소를 제어하는 클래스입니다.
//아이콘과 진행률 표시를 담당하며, 비활성화 기능도 제공합니다.
public class BuffElementPanelView : MonoBehaviour
{
    [SerializeField] private Image buffImage;   //버프 아이콘 이미지
    [SerializeField] private Image sliderImage; //버프 진행률 이미지

    //버프 아이콘과 진행률을 설정합니다.
    public void Setting(Sprite _buffSpr, float _rate)
    { 
        buffImage.sprite = _buffSpr;
        sliderImage.fillAmount = _rate;
    }

    //버프 UI를 비활성화하고 초기화합니다.
    public void Disable()
    {
        buffImage.sprite = null;
        sliderImage.fillAmount = 0;
        gameObject.SetActive(false);
    }
}
