using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 아이템과 골드의 획득, 제거, 교환을 관리하는 클래스입니다.
/// </summary>
public class ItemManager : MonoBehaviour, IItemManager
{
    //아이템 관련 이벤트
    public Action<IReadOnlyList<ItemStack>> OnItemPossessionChanged { get; set; }
    public Action<Item, int> OnGetItem { get; set; }
    public Action<Item> OnGetItemFailed { get; set; }

    //골드 관련 이벤트
    public Action<int> OnGetGold { get; set; }
    public Action<int> OnGoldAmountChanged { get; set; }

    public int GoldAmount => goldAmount;

    private static int noOverlapKey;    //중첩 불가능 아이템 고유 키 생성용
    public static int CurNoOverlapKey
    {
        get => noOverlapKey;
        set => noOverlapKey = value;
    }

    private List<ItemStack> itemStacks = new();    //보유 아이템 스택 리스트
    public IReadOnlyList<ItemStack> ItemStacks => itemStacks.AsReadOnly();

    private int goldAmount;

    private void Awake()
    {
        //서비스 로케이터에 서비스 등록
        IItemManager myInterFace = this;
        ServiceLocator.Register(myInterFace);

        noOverlapKey = 100001;
    }

    /// <summary>
    /// 아이템 스택을 조회합니다.
    /// </summary>
    public ItemStack GetItemStack(Item _item)
    {
        if (_item == null)
            return null;
        else
            return itemStacks.Find(x => x.item.ItemKey == _item.ItemKey);
    }

    /// <summary>
    /// 아이템을 획득합니다.
    /// </summary>
    public void GetItem(Item _item, int _amount, bool _isNoOverlapKeyAllocated = false)
    {
        if(_item == null) return;

        //아이템이 중복을 허용하지 않으면
        //장비 아이템은 각각의 상태(예를들면 강화상태)등이 존재하므로 중복을 허용하지 않습니다.
        if (!_item.IsOverlap)
        {
            //아이템 생성시 자동으로 키값을 할당해야한다면
            //아이템 생성하고 키값을 설정합니다.
            if (_isNoOverlapKeyAllocated)
            {
                Item cloneItem = Instantiate(_item);
                cloneItem.ItemKey = "No" + noOverlapKey;
                noOverlapKey++;
                itemStacks.Add(new ItemStack(cloneItem, 1));
            }
            //그렇지 않으면 전달받은 아이템을 키값을 변경하지않고 스택에 저장합니다.
            else
            {
                if (itemStacks.Exists(i => i.item.ItemKey == _item.ItemKey))
                {
                    Debug.LogWarning($"보유하고있는 아이템중 {_item.ItemKey}와 같은 아이템이 존재하므로 " +
                                    $"새로운 itemstack을 추가할수 없습니다.");
                    OnGetItemFailed?.Invoke(_item);
                    return;
                }

                itemStacks.Add(new ItemStack(_item, 1));
            }
        }
        else
        {
            string itemKey = _item.ItemKey;
            ItemStack targetStack = itemStacks.Find(i => i.item.ItemKey == itemKey);

            if (targetStack != null)
            {
                targetStack.amount += _amount;
            }
            else
            {
                itemStacks.Add(new ItemStack(_item, _amount));
            }
        }

        OnItemPossessionChanged?.Invoke(itemStacks.AsReadOnly());
        OnGetItem?.Invoke(_item, _amount);
    }

    /// <summary>
    /// 아이템을 제거합니다.
    /// </summary>
    public void RemoveItem(Item _item, int _amount)
    {
        string itemKey = _item.ItemKey;
        ItemStack targetStack = itemStacks.Find(i => i.item.ItemKey == itemKey);

        if (targetStack != null)
        {
            if (targetStack.amount > _amount)
            {
                targetStack.amount -= _amount;
            }
            else if (targetStack.amount == _amount)
            {
                targetStack.amount -= _amount;
                itemStacks.Remove(targetStack);
            }
            else
            {
                Debug.LogWarning($"{_item.ItemKey} 아이템이 보유갯수 이상으로 빠졌습니다.");
                itemStacks.Remove(targetStack);
            }
        }
        else
        {
            Debug.LogWarning($"{_item.ItemKey} 아이템을 보유하고있지 않지만 빠졌습니다.");
        }

        OnItemPossessionChanged?.Invoke(itemStacks.AsReadOnly());
    }

    /// <summary>
    /// 아이템 위치를 교환합니다.
    /// </summary>
    public void SwapItem(Item _itemA, Item _itemB)
    {
        if (_itemA == null || _itemB == null)
            return;

        int indexA = itemStacks.FindIndex(x => x.item.ItemKey == _itemA.ItemKey);
        int indexB = itemStacks.FindIndex(x => x.item.ItemKey == _itemB.ItemKey);

        if (indexA < 0 || indexB < 0)
            return;

        ItemStack tmp, stackA, stackB;
        stackA = itemStacks[indexA];
        stackB = itemStacks[indexB];

        tmp = stackA;
        stackA = stackB;
        stackB = tmp;

        itemStacks[indexA] = stackA;
        itemStacks[indexB] = stackB;

        OnItemPossessionChanged?.Invoke(itemStacks.AsReadOnly());
    }

    /// <summary>
    /// 골드를 획득합니다.
    /// </summary>
    public void GetGold(int _amount)
    {
        goldAmount += _amount;
        OnGetGold?.Invoke(_amount);
        OnGoldAmountChanged?.Invoke(goldAmount);
    }

    /// <summary>
    /// 골드를 사용합니다.
    /// </summary>
    public void UseGold(int _amount)
    {
        goldAmount -= _amount;
        OnGoldAmountChanged?.Invoke(goldAmount);
    }

    /// <summary>
    /// 아이템 및 골드 상태를 초기화합니다.
    /// </summary>
    public void Initial()
    {
        noOverlapKey = 100001;
        itemStacks.Clear();

        goldAmount = 0;
    }
}

/// <summary>
/// 아이템 수량과 함께 저장하는 클래스입니다.
/// </summary>
[System.Serializable]
public class ItemStack
{
    public Item item;
    public int amount;

    public ItemStack(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
