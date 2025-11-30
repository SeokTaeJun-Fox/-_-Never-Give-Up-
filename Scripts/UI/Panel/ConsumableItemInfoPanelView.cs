using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class ConsumableItemInfoPanelView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmountTmp;
    [SerializeField] private TextMeshProUGUI itemNameTmp;
    [SerializeField] private TextMeshProUGUI itemDescTmp;

    [SerializeField] private Button useButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button slotSettingButton;

    private ItemStack myItemStack;

    public Button UseButton { get => useButton; }
    public Button CloseButton { get => closeButton; }
    public Button SlotSettingButton { get => slotSettingButton; }

    public void SettingView(ItemStack _itemStack)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = _itemStack.item.Icon;
        itemAmountTmp.text = _itemStack.amount.ToString();
        itemNameTmp.text = _itemStack.item.ItemName;
        itemDescTmp.text = _itemStack.item.Desc;
    }

    public void ResetView()
    {
        itemImage.gameObject.SetActive(false);
        itemImage.sprite = null;
        itemAmountTmp.text = "";
        itemNameTmp.text = "";
        itemDescTmp.text = "";
    }

    public void ShowButton(bool _canItemUsed)
    {
        useButton.gameObject.SetActive(_canItemUsed);
        SlotSettingButton.gameObject.SetActive(_canItemUsed);
    }
}
