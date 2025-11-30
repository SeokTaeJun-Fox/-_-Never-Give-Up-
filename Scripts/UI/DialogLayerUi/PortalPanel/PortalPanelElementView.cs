using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PortalPanelElementView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI content;

    public Button Button { get => button; }

    public void SetText(string _content)
    { 
        content.text = _content;
    }
}
