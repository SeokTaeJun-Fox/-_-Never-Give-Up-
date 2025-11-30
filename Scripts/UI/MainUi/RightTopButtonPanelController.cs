using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTopButtonPanelController : MainUIElement
{
    [SerializeField] private RightTopButtonPanelView view;

    //이 클래스에 필요한 인터페이스들

    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent uiWindowFactoryEvent;

    public override void Initial()
    {
        view.SkillButton.onClick.AddListener(OnClickSkillButton);
        view.QuestButton.onClick.AddListener(OnClickQuestButton);
        view.AbilityButton.onClick.AddListener(OnClickAbilityButton);
        view.InventoryButton.onClick.AddListener(OnClickInvenButton);
        view.MenuButton.onClick.AddListener(OnClickMenuButton);
    }

    public override void InjectDependencies(object[] _dependencies)
    {

    }

    private void OnClickSkillButton()
    {
        uiWindowFactoryEvent.Raise(UIType.SKILLWINDOW);
    }

    private void OnClickAbilityButton()
    {
        uiWindowFactoryEvent.Raise(UIType.PLAYER_ABILITY_WINDOW);
    }

    private void OnClickInvenButton()
    {
        uiWindowFactoryEvent.Raise(UIType.INVENTORY);
    }

    private void OnClickQuestButton()
    {
        uiWindowFactoryEvent.Raise(UIType.QUEST_WINDOW);
    }

    private void OnClickMenuButton()
    {
        uiWindowFactoryEvent.Raise(UIType.MENU_WINDOW);
    }
}
