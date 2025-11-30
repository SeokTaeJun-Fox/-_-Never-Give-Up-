using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightTopButtonPanelView : MonoBehaviour
{
    [SerializeField] private Button skillButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button menuButton;
    
    public Button SkillButton { get => skillButton; }
    public Button QuestButton { get => questButton; }
    public Button AbilityButton { get => abilityButton; }
    public Button InventoryButton { get => inventoryButton; }
    public Button MenuButton { get => menuButton; }
}
