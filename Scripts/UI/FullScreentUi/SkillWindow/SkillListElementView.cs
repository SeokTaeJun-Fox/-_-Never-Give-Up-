using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillListElementView : MonoBehaviour
{
    [SerializeField] private Image SkillIcon;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillLevel;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button keyRegisterButton;
    [SerializeField] private Image panelImage;
    [SerializeField] private Button panelButton;

    [SerializeField] private Sprite selectedSpr;
    [SerializeField] private Sprite unSelectedSpr;

    private bool isSelected;

    public Button LevelUpButton { get => levelUpButton; }
    public Button KeyRegisterButton { get => keyRegisterButton; }
    public Button PanelButton { get => panelButton; }
    public bool IsSelected => isSelected;

    public void ShowView(Sprite _icon, string _name, int _level)
    {
        SkillIcon.sprite = _icon;
        skillName.text = _name;

        if (_level != 0)
            skillLevel.text = "LV. " + _level.ToString();
        else
            skillLevel.text = "πÃ»πµÊ";
    }

    public void ShowSelectedPanel(bool _isSelected)
    { 
        panelImage.sprite = _isSelected ? selectedSpr : unSelectedSpr;
        isSelected = _isSelected;
    }

    public void Initial()
    {
        ShowSelectedPanel(false);

        SkillIcon.sprite = null;
        skillName.text = string.Empty;
        skillLevel.text = string.Empty;
    }
}
