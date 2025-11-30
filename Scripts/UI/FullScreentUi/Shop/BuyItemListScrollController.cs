using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using System;

public class BuyItemListScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] EnhancedScrollerCellView cellViewPrefab;

    private List<BuyItemShopInfoEntry> datas;

    public event Action<BuyItemShopInfoEntry> OnClickBuyButton;
    public event Action<Item> OnClickImageButton;
    public event Func<BuyItemShopInfoEntry, bool> OnBuyButtonInteractable;

    public void LoadData(List<BuyItemShopInfoEntry> _datas)
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
        BuyItemListElementView element = scroller.GetCellView(cellViewPrefab) as BuyItemListElementView;

        if (!element.IsEventSetting)
        {
            element.SetEvent(OnClickBuyButton, OnClickImageButton);
        }

        element.SetData(datas[dataIndex], OnBuyButtonInteractable.Invoke(datas[dataIndex]));

        return element;
    }
    #endregion
}
