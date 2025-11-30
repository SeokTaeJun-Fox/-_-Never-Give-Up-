using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyItemPanel : MonoBehaviour
{
    [SerializeField] private ItemInfoPanelCreator itemInfoPanelCreator;
    [SerializeField] private BuyItemListScrollController scrollController;
    [SerializeField] private BuyItemShopInfoRegistry registry;

    //Top Toggle
    [SerializeField] private Toggle totalToggle;
    [SerializeField] private Toggle weaponToggle;
    [SerializeField] private Toggle consumeToggle;
    [SerializeField] private Toggle normalToggle;
    [SerializeField] private Toggle enchantToggle;

    [SerializeField] private UnityEvent onClickBuyButton;
    [SerializeField] private UnityEvent onClickImageButton;

    private ItemListType curShowItemType = ItemListType.TOTAL;

    //이 클래스에 필요한 스크립터블오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    private void Awake()
    {
        totalToggle.onValueChanged.AddListener(OnValueChangedTotalToggle);
        weaponToggle.onValueChanged.AddListener(OnValueChangedWeaponToggle);
        consumeToggle.onValueChanged.AddListener(OnValueChangedConsumeToggle);
        normalToggle.onValueChanged.AddListener(OnValueChangedNormalToggle);
        enchantToggle.onValueChanged.AddListener(OnValueChangedEnchantToggle);

        totalToggle.isOn = true;

        scrollController.OnBuyButtonInteractable += OnBuyButtonInteractable;
        scrollController.OnClickImageButton += OnClickListImageButton;
        scrollController.OnClickBuyButton += OnClickBuyButton;
    }

    public void OpenPanel(IItemManager _itemManager)
    { 
        itemManager = _itemManager;

        if (itemManager != null)
            itemManager.OnGoldAmountChanged += OnGoldAmountChanged;

        Show(curShowItemType);
    }

    public void ClosePanel()
    { 
    
    }

    public void Show(ItemListType _type)
    { 
        curShowItemType = _type;

        if (itemManager == null)
            return;

        List<BuyItemShopInfoEntry> listDatas = null;

        switch (_type)
        {
            case ItemListType.TOTAL:
                {
                    listDatas = registry.BuyItemInfoList;
                }
                break;
            case ItemListType.CONSUMABLE:
                {
                    listDatas = registry.BuyItemInfoList.Where(x => x.Item.ItemType == ItemType.CONSUMABLE).ToList();
                }
                break;
            case ItemListType.ENCHANT:
                {
                    listDatas = registry.BuyItemInfoList.Where(x => x.Item.ItemType == ItemType.ENCHANT).ToList();
                }
                break;
            case ItemListType.NORMAL:
                {
                    listDatas = registry.BuyItemInfoList.Where(x => x.Item.ItemType == ItemType.NORMAL).ToList();
                }
                break;
            case ItemListType.EQUIPMENT:
                {
                    listDatas = registry.BuyItemInfoList.Where(x => x.Item.ItemType == ItemType.EQUIPMENT).ToList();
                }
                break;
        }

        scrollController.LoadData(listDatas);
    }

    //이벤트
    private void OnGoldAmountChanged(int _amount)
    {
        Show(curShowItemType);
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

    //각 아이템 구매 버튼의 버튼 활성화할지 비활성화할지 현재 보유골드량을 보면서 확인합니다.
    private bool OnBuyButtonInteractable(BuyItemShopInfoEntry _entry)
    {
        if (itemManager == null)
            return false;

        return _entry.BuyCost <= itemManager.GoldAmount;
    }

    private void OnClickBuyButton(BuyItemShopInfoEntry _entry)
    {
        onClickBuyButton?.Invoke();
        factoryEvent.Raise(UIType.SELECTED_BUY_ITEM_PANEL, new object[] { _entry });
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
            scrollController.OnBuyButtonInteractable -= OnBuyButtonInteractable;
            scrollController.OnClickImageButton -= OnClickListImageButton;
            scrollController.OnClickBuyButton -= OnClickBuyButton;
        }

        if (itemManager != null)
            itemManager.OnGoldAmountChanged -= OnGoldAmountChanged;
    }
}
