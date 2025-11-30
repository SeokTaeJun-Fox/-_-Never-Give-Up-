using DG.Tweening;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellItemListElementView : EnhancedScrollerCellView
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountTmp;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button imageButton;

    private Action<ItemStack> onClickButton;
    private Action<Item> onClickImageButton;

    private ItemStack target;
    private bool isEventSetting;

    public bool IsEventSetting => isEventSetting;

    //두트윈
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> scaleTween;

    private void Awake()
    {
        sellButton.onClick.AddListener(OnClickSellButton);
        imageButton.onClick.AddListener(OnClickItemImage);

        //두트윈 연출 변수에 저장
        scaleTween = transform.DOScale(Vector2.one, 0.1f).From(new Vector2(0.5f, 0.5f));
        scaleTween.SetAutoKill(false);
        scaleTween.Pause();
    }

    private void OnEnable()
    {
        scaleTween.Restart();
    }

    private void OnDisable()
    {
        if (transform != null)
            transform.localScale = Vector2.one;
    }

    public void SetData(ItemStack _itemStack)
    {
        target = _itemStack;

        if (_itemStack != null && _itemStack.item != null)
        {
            itemCost.text = _itemStack.item.SellCost.ToString();
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = _itemStack.item.Icon;
            itemName.text = _itemStack.item.ItemName;

            if (_itemStack.item.IsOverlap)
                amountTmp.text = _itemStack.amount.ToString();
            else
                amountTmp.text = "";
        }

        //초기화
        if (_itemStack == null || _itemStack.item == null)
        {
            Initial();
        }
    }

    public void SetEvent(Action<ItemStack> _onClickSellButton, Action<Item> _onClickImageButton)
    {
        onClickButton = _onClickSellButton;
        onClickImageButton = _onClickImageButton;
        isEventSetting = true;
    }

    public void Initial()
    {
        itemImage.gameObject.SetActive(false);
        amountTmp.text = "";
        itemName.text = "";
        itemCost.text = "";
        sellButton.onClick.RemoveAllListeners();
        imageButton.onClick.RemoveAllListeners();
        isEventSetting = false;
    }

    private void OnClickSellButton()
    {
        onClickButton?.Invoke(target);
    }

    private void OnClickItemImage()
    {
        if (target != null && target.item != null)
            onClickImageButton?.Invoke(target.item);
    }
}
