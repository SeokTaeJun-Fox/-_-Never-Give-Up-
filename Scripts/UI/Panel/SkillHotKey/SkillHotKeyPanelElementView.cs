using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillHotKeyPanelElementView : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private Button button;
    [SerializeField] private Image fadeImage;
    private bool isHide;

    public Button GetButton => button;
    public bool IsHide => isHide;

    public void ShowView(Sprite _skillSpr)
    {
        if (_skillSpr != null)
        {
            skillImage.enabled = true;
            skillImage.sprite = _skillSpr;
            isHide = false;
        }
        else
        {
            Hide();
            fadeImage.fillAmount = 0;
        }
    }

    public void Hide()
    {
        skillImage.enabled = false;
        isHide = true;
    }

    public void SetSlice(float _rate)
    {
        fadeImage.fillAmount = _rate;
    }
}
