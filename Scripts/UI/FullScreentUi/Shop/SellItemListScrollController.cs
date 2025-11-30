using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItemListScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] EnhancedScrollerCellView cellViewPrefab;

    private List<ItemStack> datas;

    public event Action<ItemStack> OnClickSellButton;
    public event Action<Item> OnClickImageButton;

    public void LoadData(List<ItemStack> _datas)
    {
        scroller.Delegate = this;

        datas = _datas;
        scroller.ReloadData();
    }

    #region interface
    //ºø∫‰ ªÁ¿Ã¡Ó (y√‡)
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100f;
    }

    //ºø ∞πºˆ
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return datas.Count;
    }

    //ºø µ•¿Ã≈Õ ¡÷¿‘
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        SellItemListElementView element = scroller.GetCellView(cellViewPrefab) as SellItemListElementView;

        if (!element.IsEventSetting)
        {
            element.SetEvent(OnClickSellButton, OnClickImageButton);
        }

        element.SetData(datas[dataIndex]);

        return element;
    }
    #endregion
}
