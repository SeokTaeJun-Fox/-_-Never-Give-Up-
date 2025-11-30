using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NoticePanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentTmp;
    [SerializeField] private RectTransform panel;

    public TextMeshProUGUI ContentTmp { get => contentTmp; }
    public RectTransform Panel { get => panel; }
}
