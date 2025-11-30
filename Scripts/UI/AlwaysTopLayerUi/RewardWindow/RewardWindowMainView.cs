using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardWindowMainView : MonoBehaviour
{
    [SerializeField] private Button skipButton;
    [SerializeField] private Button checkButton;
    [SerializeField] private ScrollRect scrollRect;

    public Button SkipButton { get => skipButton; }
    public Button CheckButton { get => checkButton; }
    public ScrollRect ScrollRect { get => scrollRect; }

    public void ActiveButton(bool _isActiveSkipButton)
    {
        skipButton.gameObject.SetActive(_isActiveSkipButton);
        checkButton.gameObject.SetActive(!_isActiveSkipButton);
    }
}
