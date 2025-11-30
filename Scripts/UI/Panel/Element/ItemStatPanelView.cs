using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//TMP 2개(스텟명, 스텟내용)가 들어있는 UI클래스입니다.
public class ItemStatPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statCategoryTmp;
    [SerializeField] private TextMeshProUGUI statContentTmp;

    public void ShowView(string _category, string _statContent, Color _textColor)
    { 
        statCategoryTmp.text = _category;
        statContentTmp.color = _textColor;

        statContentTmp.text = _statContent;
        statCategoryTmp.color = _textColor; 
    }

    public void Initial()
    {
        statCategoryTmp.text = "";
        statContentTmp.text = "";
    }
}
