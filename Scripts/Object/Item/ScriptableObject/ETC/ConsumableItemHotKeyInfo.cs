using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ConsumableItemHotKeyInfo", menuName = "Scriptable Object/Item/ETC/ConsumableItemHotKeyInfo")]
public class ConsumableItemHotKeyInfo : ResettableScriptableObject
{
    [SerializeField] private List<ConsumableItem> items;

    public event Action<int, ConsumableItem> OnChangeItem;

    public IReadOnlyList<ConsumableItem> Items => items;

    //핫키에 저장된 아이템을 불러옵니다.
    public ConsumableItem GetItem(int keyIndex)
    { 
        if(items.Count <= keyIndex)
            return null;
        else 
            return items[keyIndex];
    }

    public void SetItem(int keyIndex, ConsumableItem item)
    { 
        if(items.Count > keyIndex)
            items[keyIndex] = item;

        OnChangeItem?.Invoke(keyIndex, item);
    }

    public void ResetEvent()
    {
        OnChangeItem = null;
    }

    public void ClearData()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i] = null;
            OnChangeItem?.Invoke(i, null);
        }
    }

    public override void Initial()
    {
        ResetEvent();
        ClearData();
    }
}
