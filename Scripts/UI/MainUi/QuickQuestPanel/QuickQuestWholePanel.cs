using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickQuestWholePanel : MainUIElement
{
    [SerializeField] private Button panelCloseOpenButton;
    [SerializeField] private GameObject QuestPanel;

    public override void Initial()
    {
        panelCloseOpenButton.onClick.AddListener(OnClickPanelCloseOpenButton);
    }

    public override void InjectDependencies(object[] _dependencies)
    {

    }

    private void OnClickPanelCloseOpenButton()
    {
        QuestPanel.SetActive(!QuestPanel.activeInHierarchy);
    }
}
