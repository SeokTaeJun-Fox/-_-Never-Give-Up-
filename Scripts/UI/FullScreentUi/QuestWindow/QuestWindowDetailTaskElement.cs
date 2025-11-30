using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestWindowDetailTaskElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentTmp;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color completeColor;

    public void Setting(string _content, bool _isComplete)
    { 
        contentTmp.color = !_isComplete ? normalColor : completeColor;
        contentTmp.text = _content;
    }
}
