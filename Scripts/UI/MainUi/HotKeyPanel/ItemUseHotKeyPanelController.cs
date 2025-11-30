using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseHotKeyPanelController : MainUIElement
{
    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    //이 클래스에 필요한 스크립터블오브젝트들
    [SerializeField] private ConsumableItemHotKeyInfo keyInfo;
    [SerializeField] private GetIItemUserEvent getIItemUserEvent;

    [SerializeField] private ItemHotKeyPanelElement[] hotKeyPanels;
    [SerializeField] private KeyAndHotkeyPanelElement[] keyAndPanels;

    private void Update()
    {
        if (itemManager == null)
        {
            return;
        }

        ConsumableItem target = null;

        for (int i = 0; i < keyAndPanels.Length; i++)
        {
            if (Input.GetKeyDown(keyAndPanels[i].keyCode))
            {
                target = keyAndPanels[i].panelElement.TargetItem;
                if (target != null && itemManager.GetItemStack(target) != null)
                {
                    target.Use(getIItemUserEvent.RaiseGet()); //임시
                    itemManager.RemoveItem(target, 1);
                }
            }
        }
    }

    public override void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IItemManager item)
                itemManager = item;
        }
    }

    public override void Initial()
    {
        if(itemManager != null)
            itemManager.OnItemPossessionChanged += OnItemPossessionChanged;

        keyInfo.OnChangeItem += OnChangedItem;

        UpdateAllPanel();
    }

    private void UpdateAllPanel()
    {
        for (int i = 0; i < hotKeyPanels.Length; i++)
        {
            ConsumableItem item = keyInfo.GetItem(i);

            ItemStack itemStack;
            if (item == null)
            {
                hotKeyPanels[i].Hide();
            }
            else
            {
                itemStack = itemManager.GetItemStack(item);
                if (itemStack == null)  //해당 아이템이 슬롯에 등록되었지만 아이템매니저 보유항목에 없으면 아이템갯수는 0입니다.
                {
                    hotKeyPanels[i].ShowView(item, 0);
                }
                else
                {
                    hotKeyPanels[i].ShowView(item, itemStack.amount);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(itemManager != null)
            itemManager.OnItemPossessionChanged -= OnItemPossessionChanged;

        keyInfo.OnChangeItem -= OnChangedItem;
    }

    //이벤트
    private void OnItemPossessionChanged(IReadOnlyList<ItemStack> _itemStacks)
    {
        UpdateAllPanel();
    }

    private void OnChangedItem(int _elementIndex, ConsumableItem _item)
    {
        if (itemManager == null)
            return;

        if (hotKeyPanels.Length > _elementIndex)
        {
            //단축키에 아이템을 등록해제했다면
            if (_item == null)
                hotKeyPanels[_elementIndex].Hide();
            
            //그렇지 않다면
            else
            {
                ItemStack itemStack = itemManager.GetItemStack(_item);
                if (itemStack == null)  //해당 아이템이 슬롯에 등록되었지만 아이템매니저 보유항목에 없으면 아이템갯수는 0입니다.
                {
                    hotKeyPanels[_elementIndex].ShowView(_item, 0);
                }
                else
                {
                    hotKeyPanels[_elementIndex].ShowView(_item, itemStack.amount);
                }
            }
        }
    }
}

[Serializable]
public struct KeyAndHotkeyPanelElement
{
    public KeyCode keyCode;
    public ItemHotKeyPanelElement panelElement;
}
