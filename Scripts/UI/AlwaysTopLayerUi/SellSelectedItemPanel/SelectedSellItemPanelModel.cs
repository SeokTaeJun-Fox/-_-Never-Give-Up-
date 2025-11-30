using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSellItemPanelModel : MonoBehaviour
{
    private Item targetItem;
    private int sellCost;
    private int curAmount;
    private int maxAmount;

    public event Action<int> OnCurAmountChanged;

    public void SetInfo(ItemStack _itemStack, int _curAmount)
    {
        targetItem = _itemStack.item;
        sellCost = _itemStack.item.SellCost;
        curAmount = _curAmount;
        maxAmount = _itemStack.amount;

        OnCurAmountChanged?.Invoke(curAmount);
    }

    public void SetCurAmount(int _amount)
    {
        curAmount = _amount;
        OnCurAmountChanged?.Invoke(curAmount);
    }

    public Item TargetItem { get => targetItem; }
    public int SellCost { get => sellCost; }
    public int CurAmount { get => curAmount; }
    public int MaxAmount { get => maxAmount; }
}
