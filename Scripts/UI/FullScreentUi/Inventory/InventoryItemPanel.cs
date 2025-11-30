using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryItemPanel : MonoBehaviour
{
    [SerializeField] private List<InventoryElement> elements = new List<InventoryElement>();
    [SerializeField] private ItemInfoPanelCreator itemInfoPanelCreator;

    private IReadOnlyList<ItemStack> allItemStacks;  //모든 아이템 스택 리스트
    private List<ItemStack> showItemStacks = new List<ItemStack>(); //조건에 맞은 아이템 스택 리스트

    //이 클래스에 필요한 인터페이스
    private IItemManager itemManager;
    private IEquipmentManager equipmentManager;

    //page button
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private TextMeshProUGUI pageTmp;

    //Top Toggle
    [SerializeField] private Toggle totalToggle;
    [SerializeField] private Toggle weaponToggle;
    [SerializeField] private Toggle consumeToggle;
    [SerializeField] private Toggle normalToggle;
    [SerializeField] private Toggle enchantToggle;

    private int page;
    private int maxPage;
    private ItemListType curShowItemType = ItemListType.TOTAL;

    private int curShowPage;

    private void Awake()
    {
        totalToggle.onValueChanged.AddListener(OnValueChangedTotalToggle);
        weaponToggle.onValueChanged.AddListener(OnValueChangedWeaponToggle);
        consumeToggle.onValueChanged.AddListener(OnValueChangedConsumeToggle);
        normalToggle.onValueChanged.AddListener(OnValueChangedNormalToggle);
        enchantToggle.onValueChanged.AddListener(OnValueChangedEnchantToggle);

        upButton.onClick.AddListener(OnClickUpButton);
        downButton.onClick.AddListener(OnClickDownButton);

        foreach (var itemButton in elements)
        {
            itemButton.OnClickButton += OnItemButtonClick;
            itemButton.OnDropped += OnItemDropped;
        }

        totalToggle.isOn = true;
        curShowPage = 1;
    }

    public void Initial(IItemManager _itemManager, IEquipmentManager _equipmentManager)
    {
        itemManager = _itemManager;
        equipmentManager = _equipmentManager;

        if (itemManager != null)
        {
            itemManager.OnItemPossessionChanged += OnItemPossessedChanged;
            Setting(itemManager.ItemStacks);
        }

        if (equipmentManager != null)
            equipmentManager.OnEquipChanged += OnEquipChanged;
    }

    public void Setting(IReadOnlyList<ItemStack> _allItemStacks)
    {
        allItemStacks = _allItemStacks;

        Show(curShowItemType, curShowPage);
    }

    public void Show(ItemListType _showType, int _page)
    {
        showItemStacks.Clear();

        switch (_showType)
        {
            case ItemListType.TOTAL:
                {
                    showItemStacks = allItemStacks.ToList();
                }
                break;
            case ItemListType.CONSUMABLE:
                { 
                    showItemStacks = allItemStacks.Where(x => x.item.ItemType == ItemType.CONSUMABLE).ToList();
                }
                break;
            case ItemListType.ENCHANT:
                {
                    showItemStacks = allItemStacks.Where(x => x.item.ItemType == ItemType.ENCHANT).ToList();
                }
                break;
            case ItemListType.NORMAL:
                {
                    showItemStacks = allItemStacks.Where(x => x.item.ItemType == ItemType.NORMAL).ToList();
                }
                break;
            case ItemListType.EQUIPMENT:
                {
                    showItemStacks = allItemStacks.Where(x => x.item.ItemType == ItemType.EQUIPMENT).ToList();
                }
                break;
        }

        int curIndex;

        if (curShowPage != 1 && showItemStacks.Count <= (_page - 1) * elements.Count)
        {
            _page--;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            curIndex = (_page - 1) * elements.Count + i;
            if (showItemStacks.Count > curIndex)
            {
                elements[i].Setting(showItemStacks[curIndex]);
                elements[i].EquipSignUpdate();
            }
            else
            {
                elements[i].Clear();
            }
        }

        curShowPage = _page;
        page = _page;
        maxPage = Mathf.CeilToInt((float)showItemStacks.Count / elements.Count);//(showItemStacks.Count / elements.Count) + 1;

        if (maxPage == 0)
            maxPage = 1;
        
        curShowItemType = _showType;
        PageViewUpdate(page, maxPage);
    }

    private void PageViewUpdate(int _curPage, int _maxPage)
    {
        if (_curPage == 1)
        {
            upButton.interactable = false;
        }
        else
        {
            upButton.interactable = true;
        }

        if (_curPage < maxPage)
        {
            downButton.interactable = true;
        }
        else
        {
            downButton.interactable= false;
        }

        pageTmp.text = $"{_curPage} / {_maxPage}";
    }

    public void OnCloseInven()
    {
        if (itemManager != null)
            itemManager.OnItemPossessionChanged -= OnItemPossessedChanged;

        if (equipmentManager != null)
            equipmentManager.OnEquipChanged -= OnEquipChanged;
    }

    private void OnDestroy()
    {
        if (itemManager != null)
            itemManager.OnItemPossessionChanged -= OnItemPossessedChanged;

        if (equipmentManager != null)
            equipmentManager.OnEquipChanged -= OnEquipChanged;
    }

    //이벤트
    private void OnValueChangedTotalToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.TOTAL, 1);
        }
    }

    private void OnValueChangedWeaponToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.EQUIPMENT, 1);
        }
    }

    private void OnValueChangedConsumeToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.CONSUMABLE, 1);
        }
    }

    private void OnValueChangedNormalToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.NORMAL, 1);
        }
    }

    private void OnValueChangedEnchantToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.ENCHANT, 1);
        }
    }

    private void OnClickUpButton()
    {
        Show(curShowItemType, page - 1);
    }

    private void OnClickDownButton()
    {
        Show(curShowItemType, page + 1);
    }

    private void OnItemButtonClick(ItemStack _itemStack)
    {
        if (_itemStack != null)
        {
            itemInfoPanelCreator.OpenPanel(_itemStack);
        }
    }

    private void OnItemDropped(Item _itemA, Item _itemB)
    { 
        if(itemManager != null)
            itemManager.SwapItem(_itemA, _itemB);
    }

    private void OnItemPossessedChanged(IReadOnlyList<ItemStack> _itemStacks)
    {
        Setting(_itemStacks);
    }

    private void OnEquipChanged(EquipmentType _type, EquipmentItem _item)
    {
        foreach (var element in elements)
            element.EquipSignUpdate();
    }
}
