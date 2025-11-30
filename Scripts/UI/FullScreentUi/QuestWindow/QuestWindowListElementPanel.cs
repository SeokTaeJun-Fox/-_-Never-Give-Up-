using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestWindowListElementPanel : MonoBehaviour
{
    [SerializeField] private Image questIcon;
    [SerializeField] private TextMeshProUGUI contentTmp;
    [SerializeField] Toggle toggle;

    public Toggle Toggle { get => toggle; }

    public void Setting(Sprite _questIcon, string _content)
    {
        questIcon.sprite = _questIcon;
        contentTmp.text = _content;
    }
}
