using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupWindowView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI contentTmp;
    [SerializeField] private TextMeshProUGUI yesTmp;
    [SerializeField] private TextMeshProUGUI noTmp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public Button YesButton { get => yesButton; }
    public Button NoButton { get => noButton; }

    public void SetTitle(string _title)
    { 
        titleTmp.text = _title;
    }

    public void SetContent(string _content)
    { 
        contentTmp.text = _content;
    }

    public void ActiveButton(bool _isActiveTwoButton)
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(_isActiveTwoButton);
    }

    public void SetYesButtonText(string _text)
    { 
        yesTmp.text = _text;
    }

    public void SetNoButtonText(string _text)
    { 
        noTmp.text = _text;
    }
}
