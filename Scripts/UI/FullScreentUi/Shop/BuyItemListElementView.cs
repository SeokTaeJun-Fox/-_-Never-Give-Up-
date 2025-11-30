using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class BuyItemListElementView : EnhancedScrollerCellView
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button imageButton;

    private Action<BuyItemShopInfoEntry> onClickButton;
    private Action<Item> onClickImageButton;

    private BuyItemShopInfoEntry target;
    private bool isEventSetting;

    public bool IsEventSetting => isEventSetting;

    //두트윈
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> scaleTween;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnClickBuyButton);
        imageButton.onClick.AddListener(OnClickItemImage);

        //두트윈 연출 변수에 저장
        scaleTween = transform.DOScale(Vector2.one, 0.1f).From(new Vector2(0.5f, 0.5f));
        scaleTween.SetAutoKill(false);
        scaleTween.Pause();
    }

    private void OnEnable()
    {
        //오브젝트가 활성화될때마다 두트윈 연출
        scaleTween.Restart();
    }

    private void OnDisable()
    {
        if(transform != null)
            transform.localScale = Vector2.one;
    }

    public void SetData(BuyItemShopInfoEntry _entry, bool _isInteractableBuyButton = true)
    { 
        target = _entry;

        if (_entry != null)
        {
            itemCost.text = _entry.BuyCost.ToString();
        }

        if (_entry != null && _entry.Item != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = _entry.Item.Icon;
            itemName.text = _entry.Item.ItemName;
        }

        buyButton.interactable = _isInteractableBuyButton;

        //초기화
        if (_entry == null || _entry.Item == null)
        {
            Initial();
        }
    }

    public void SetEvent(Action<BuyItemShopInfoEntry> _onClickBuyButton, Action<Item> _onClickImageButton)
    { 
        onClickButton = _onClickBuyButton;
        onClickImageButton = _onClickImageButton;
        isEventSetting = true;
    }

    public void Initial()
    {
        itemImage.gameObject.SetActive(false);
        itemName.text = "";
        itemCost.text = "";
        buyButton.onClick.RemoveAllListeners();
        imageButton.onClick.RemoveAllListeners();
        isEventSetting = false;
    }

    private void OnClickBuyButton()
    {
        onClickButton?.Invoke(target);
    }

    private void OnClickItemImage()
    {
        if(target != null && target.Item != null)
            onClickImageButton?.Invoke(target.Item);
    }
}
