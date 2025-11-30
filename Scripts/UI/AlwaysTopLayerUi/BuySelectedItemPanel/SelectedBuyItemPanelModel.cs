using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectedBuyItemPanelModel : MonoBehaviour
{
    private Item targetItem;
    private int buyCost;
    private int curAmount;
    private int maxAmount;

    public event Action<int> OnCurAmountChanged;

    public void SetInfo(BuyItemShopInfoEntry _entry, int _curAmount, int _maxAmount)
    {
        targetItem = _entry.Item;
        buyCost = _entry.BuyCost;
        curAmount = _curAmount;
        maxAmount = _maxAmount;

        OnCurAmountChanged?.Invoke(curAmount);
    }

    public void SetCurAmount(int _amount)
    { 
        curAmount = _amount;
        OnCurAmountChanged?.Invoke(curAmount);
    }

    public Item TargetItem { get => targetItem; }
    public int BuyCost { get => buyCost; }
    public int CurAmount { get => curAmount; }
    public int MaxAmount { get => maxAmount; }
}
