using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListElementWindowView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contentTmp;
    [SerializeField] Button button;

    public Button Button { get => button; }

    public void SetContent(string _content)
    { 
        contentTmp.text = _content;
    }
}
