using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text;
using System;

/// <summary>
/// 대화창 UI를 제어하는 클래스입니다.
/// 화자 정보, 대사 내용, 버튼 패널 등을 표시하며,
/// 최적화를 위해 StringBuilder를 활용해 텍스트 타이핑을 구현했습니다.
/// </summary>
public class DialogPanelView : MonoBehaviour
{
    [SerializeField] private Image talkerImage;
    [SerializeField] private TextMeshProUGUI talkerNameTmp;
    [SerializeField] private TextMeshProUGUI contentTmp;
    [SerializeField] private GameObject nextObject;
    [SerializeField] private Button contentPanelButton;

    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private Button questButton;

    [SerializeField] float typingSpeed;

    private StringBuilder sb = new StringBuilder();
    private Coroutine typeTextCo;
    private bool isTypeEnd;
    private string curFullText;

    public bool IsTypeEnd => isTypeEnd;

    public Button QuestButton { get => questButton; }
    public Button ContentPanelButton { get => contentPanelButton; }

    //대화 데이터를 받아 UI에 표시하고 타이핑 효과를 시작합니다.
    public void ShowDialog(DialogueLine _data, bool _isShowButtonPanelAfterFinished = false)
    {
        talkerImage.sprite = _data.talkerSpr;
        talkerNameTmp.text = _data.talker;
        isTypeEnd = false;

        if(typeTextCo != null)
            StopCoroutine(typeTextCo);

        typeTextCo = StartCoroutine(CoTypeText(_data.content, _isShowButtonPanelAfterFinished));
    }

    //텍스트를 한 글자씩 출력하는 타이핑 코루틴입니다.
    IEnumerator CoTypeText(string _fullText, bool _isShowButtonPanelAfterFinished = false)
    {
        curFullText = _fullText;

        sb.Clear();
        contentTmp.text = "";
        nextObject.SetActive(false);

        for (int i = 0; i < _fullText.Length; i++)
        {
            sb.Append(_fullText[i]);
            contentTmp.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }

        nextObject.SetActive(true);
        isTypeEnd = true;

        if (_isShowButtonPanelAfterFinished)
            buttonPanel.SetActive(true);
    }

    //타이핑 효과를 즉시 완료하고 전체 텍스트를 표시합니다.
    public void SkipTyping()
    {
        if (!isTypeEnd)
        {
            StopCoroutine(typeTextCo);
            contentTmp.text = curFullText;
            nextObject.SetActive(true);
            isTypeEnd = true;
        }
    }

    //화자의 이미지와 이름을 표시합니다.
    public void ShowTalkerInfo(Sprite _talkerSpr, String _talkerName)
    {
        talkerImage.sprite = _talkerSpr;
        talkerNameTmp.text= _talkerName;
    }

    //대화창에 텍스트를 직접 설정합니다. (타이핑 없이 즉시 표시)
    public void SetContent(string _content)
    { 
        contentTmp.text = _content;
    }

    public void ShowButtonPanel(bool _isActive)
    { 
        buttonPanel.SetActive(_isActive);
    }

    public void InitialView()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);

            talkerImage.sprite = null;
            talkerNameTmp.text = "";
            isTypeEnd = true;
            curFullText = "";
            contentTmp.text = "";
            ShowButtonPanel(false);
        }
    }
}
