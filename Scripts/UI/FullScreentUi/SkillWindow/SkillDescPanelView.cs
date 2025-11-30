using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillDescPanelView : MonoBehaviour
{
    [SerializeField] private GameObject curSkillDescPanel;
    [SerializeField] private TextMeshProUGUI curSkillNameTmp;
    [SerializeField] private TextMeshProUGUI curSkillMp;
    [SerializeField] private TextMeshProUGUI curSkillDesc;

    [SerializeField] private GameObject arrowObject;

    [SerializeField] private GameObject nextSkillDescPanel;
    [SerializeField] private TextMeshProUGUI nextSkillNameTmp;
    [SerializeField] private TextMeshProUGUI nextSkillMp;
    [SerializeField] private TextMeshProUGUI nextSkillDesc;

    public void ShowCurSkillDesc(string _name, string _mp, string _desc)
    {
        curSkillDescPanel.SetActive(true);
        curSkillNameTmp.text = _name;
        curSkillMp.text = "MP家葛 : " + _mp;
        curSkillDesc.text = _desc;

        arrowObject.SetActive(false);

        nextSkillDescPanel.SetActive(false);
    }

    public void ShowNextSkillDesc(string _name, string _mp, string _desc)
    {
        curSkillDescPanel.SetActive(false);

        arrowObject.SetActive(true);

        nextSkillDescPanel.SetActive(true);
        nextSkillNameTmp.text = _name;
        nextSkillMp.text = "MP家葛 : " + _mp;
        nextSkillDesc.text = _desc;
    }

    public void ShowCurNextSkillDesc(string _curName, string _curMp, string _curDesc, string _nextName, string _nextMp, string _nextDesc)
    {
        curSkillDescPanel.SetActive(true);
        curSkillNameTmp.text = _curName;
        curSkillMp.text = "MP家葛 : " + _curMp;
        curSkillDesc.text = _curDesc;

        arrowObject.SetActive(true);

        nextSkillDescPanel.SetActive(true);
        nextSkillNameTmp.text = _nextName;
        nextSkillMp.text = "MP家葛 : " + _nextMp;
        nextSkillDesc.text = _nextDesc;
    }

    public void Hide()
    {
        curSkillDescPanel.SetActive(false);
        arrowObject.SetActive(false);
        nextSkillDescPanel.SetActive(false);
    }
}
