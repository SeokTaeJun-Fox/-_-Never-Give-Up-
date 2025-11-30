using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SellItemPanel : MonoBehaviour
{
    [SerializeField] private ItemInfoPanelCreator itemInfoPanelCreator;
    [SerializeField] private SellItemListScrollController scrollController;

    //Top Toggle
    [SerializeField] private Toggle totalToggle;
    [SerializeField] private Toggle weaponToggle;
    [SerializeField] private Toggle consumeToggle;
    [SerializeField] private Toggle normalToggle;
    [SerializeField] private Toggle enchantToggle;

    [SerializeField] private UnityEvent onClickSellButton;
    [SerializeField] private UnityEvent onClickImageButton;

    private ItemListType curShowItemType = ItemListType.TOTAL;

    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent uiFactoryEvent;

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;
    private IEquipmentManager equipmentManager;

    private void Awake()
    {
        totalToggle.onValueChanged.AddListener(OnValueChangedTotalToggle);
        weaponToggle.onValueChanged.AddListener(OnValueChangedWeaponToggle);
        consumeToggle.onValueChanged.AddListener(OnValueChangedConsumeToggle);
        normalToggle.onValueChanged.AddListener(OnValueChangedNormalToggle);
        enchantToggle.onValueChanged.AddListener(OnValueChangedEnchantToggle);

        totalToggle.isOn = true;

        scrollController.OnClickSellButton += OnClickSellButton;
        scrollController.OnClickImageButton += OnClickListImageButton;
    }

    public void OpenPanel(IItemManager _itemManager, IEquipmentManager _equipmentManager)
    {
        itemManager = _itemManager;
        equipmentManager = _equipmentManager;

        if (itemManager != null)
        { 
            itemManager.OnItemPossessionChanged += OnItemPossessionChanged;
        }
        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged += OnEquipChanged;
        }

        Show(curShowItemType);
    }

    public void ClosePanel()
    {
        if (itemManager != null)
        {
            itemManager.OnItemPossessionChanged -= OnItemPossessionChanged;
        }
        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged -= OnEquipChanged;
        }
    }

    public void Show(ItemListType _type)
    {
        curShowItemType = _type;

        if (itemManager == null || equipmentManager == null)
            return;

        List<ItemStack> datas = new List<ItemStack>();
        datas = itemManager.ItemStacks.ToList();
        datas = datas.Where(x =>
                                x.item.ItemType != ItemType.EQUIPMENT ||        //이 아이템이 장비아이템이 아니거나
                                (x.item is EquipmentItem &&                     //장비아이템이여도 착용되지 않는 상태이면 저장됩니다.
                                ((EquipmentItem)x.item).IsEquipped == false)
        ).ToList();

        switch (_type)
        {
            case ItemListType.TOTAL:
                {
                    
                }
                break;
            case ItemListType.CONSUMABLE:
                {
                    datas = datas.Where(x => x.item.ItemType == ItemType.CONSUMABLE).ToList();
                }
                break;
            case ItemListType.ENCHANT:
                {
                    datas = datas.Where(x => x.item.ItemType == ItemType.ENCHANT).ToList();
                }
                break;
            case ItemListType.NORMAL:
                {
                    datas = datas.Where(x => x.item.ItemType == ItemType.NORMAL).ToList();
                }
                break;
            case ItemListType.EQUIPMENT:
                {
                    datas = datas.Where(x => x.item.ItemType == ItemType.EQUIPMENT).ToList();
                }
                break;
        }

        scrollController.LoadData(datas);
    }

    private void OnValueChangedTotalToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.TOTAL);
        }
    }

    private void OnValueChangedWeaponToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.EQUIPMENT);
        }
    }

    private void OnValueChangedConsumeToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.CONSUMABLE);
        }
    }

    private void OnValueChangedNormalToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.NORMAL);
        }
    }

    private void OnValueChangedEnchantToggle(bool _isOn)
    {
        if (_isOn)
        {
            Show(ItemListType.ENCHANT);
        }
    }

    private void OnEquipChanged(EquipmentType type, EquipmentItem item)
    {
        Show(curShowItemType);
    }

    private void OnItemPossessionChanged(IReadOnlyList<ItemStack> _itemStacks)
    {
        Show(curShowItemType);
    }

    private void OnClickSellButton(ItemStack _itemStack)
    {
        onClickSellButton?.Invoke();
        uiFactoryEvent.Raise(UIType.SELECTED_SELL_ITEM_PANEL, new object[] { _itemStack });
    }

    private void OnClickListImageButton(Item _item)
    {
        if (itemManager == null)
            return;

        ItemStack itemStack = itemManager.GetItemStack(_item);

        if (itemStack == null)
        {
            itemStack = new ItemStack(_item, 0);
        }

        itemInfoPanelCreator.OpenPanel(itemStack);
        onClickImageButton?.Invoke();
    }

    private void OnDestroy()
    {
        if (scrollController != null)
        {
            scrollController.OnClickSellButton -= OnClickSellButton;
            scrollController.OnClickImageButton -= OnClickListImageButton;
        }

        if (itemManager != null)
        {
            itemManager.OnItemPossessionChanged -= OnItemPossessionChanged;
        }
        if (equipmentManager != null)
        {
            equipmentManager.OnEquipChanged -= OnEquipChanged;
        }
    }
}
