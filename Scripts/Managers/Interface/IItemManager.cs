using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 아이템 및 골드 관리 기능을 정의하는 인터페이스입니다.
/// 인벤토리 시스템과 UI에 연결되는 이벤트를 포함합니다.
/// </summary>
public interface IItemManager
{
    IReadOnlyList<ItemStack> ItemStacks { get; }

    Action<IReadOnlyList<ItemStack>> OnItemPossessionChanged { get; set; }
    Action<Item, int> OnGetItem { get; set; }
    Action<Item> OnGetItemFailed { get; set; }
    Action<int> OnGetGold { get; set; }
    Action<int> OnGoldAmountChanged { get; set; }
    int GoldAmount { get; }

    ItemStack GetItemStack(Item _item);
    void GetItem(Item _item, int _amount, bool _isNoOverlapKeyAllocated = false);
    void RemoveItem(Item _item, int _amount);
    void SwapItem(Item _itemA, Item _itemB);
    void GetGold(int _amount);
    void UseGold(int _amount);

    void Initial();
}
