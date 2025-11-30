using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPointPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillPointTmp;

    public void ShowSkillPoint(string _point)
    { 
        skillPointTmp.text = _point;
    }
}
