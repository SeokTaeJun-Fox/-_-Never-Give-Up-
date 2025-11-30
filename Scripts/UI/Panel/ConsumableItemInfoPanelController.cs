using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableItemInfoPanelController : MonoBehaviour, IUIWindow
{
    [SerializeField] private ConsumableItemInfoPanelView view;
    private ItemStack itemStack;

    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    //이 클래스에 필요한 이벤트스크립트오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent uiFactoryEvent;
    [SerializeField] private ReportOpenWindowEvent openEvent;
    [SerializeField] private GetIItemUserEvent getIItemUserEvent;

    private void Awake()
    {
        view.CloseButton.onClick.AddListener(OnClickCloseButton);
        view.UseButton.onClick.AddListener(OnClickUseButton);
        view.SlotSettingButton.onClick.AddListener(OnClickSlotSettingButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IItemManager item)
                itemManager = item;
        }
    }

    public void Initial(object[] _datas)
    {
        foreach (var data in _datas)
        {
            if (data is ItemStack stack)
            {
                itemStack = stack;
                view.SettingView(itemStack);
                view.ShowButton(itemStack != null && itemStack.amount > 0);
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        //if(itemManager != null)
        itemManager.OnItemPossessionChanged += OnItemPossessedChanged;
    }

    public void Close()
    {
        //if (itemManager != null)
            itemManager.OnItemPossessionChanged -= OnItemPossessedChanged;
        view.ResetView();
        gameObject.SetActive(false);

        onClose?.Invoke();
    }

    //이벤트
    private void OnItemPossessedChanged(IReadOnlyList<ItemStack> _itemStacks)
    {
        if (itemStack.amount == 0)
        {
            if (openEvent != null)
                openEvent.RaiseCloseWindow();

            Close();
        }

        if(view != null)
            view.SettingView(itemStack);
    }

    private void OnClickCloseButton()
    {
        if (openEvent != null)
            openEvent.RaiseCloseWindow();

        Close();
    }

    private void OnClickUseButton()
    {
        ConsumableItem item = (ConsumableItem)itemStack.item;
        item.Use(getIItemUserEvent.RaiseGet());   //임시

        if (itemManager != null)
            itemManager.RemoveItem(itemStack.item, 1);
    }

    private void OnClickSlotSettingButton()
    {
        uiFactoryEvent.Raise(UIType.CONSUMABLE_ITEM_SLOT_WINDOW, new object[] { itemStack.item });
    }
}
