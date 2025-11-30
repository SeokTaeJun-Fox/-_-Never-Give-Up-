using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LeftNoticeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color showColor;
    private bool isShow;

    public bool IsShow => isShow;

    public void ShowText(string _content)
    {
        if (gameObject.activeSelf)
        {
            text.DOKill();
        }

        gameObject.SetActive(true);

        text.text = _content;
        text.color = showColor;
        isShow = true;

        text.DOFade(0, 3f).SetDelay(3f).OnComplete(() =>
        {
            isShow = false;
            gameObject.SetActive(false);
        });
    }
}
