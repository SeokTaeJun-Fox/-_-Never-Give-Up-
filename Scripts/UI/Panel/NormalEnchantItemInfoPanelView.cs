using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalEnchantItemInfoPanelView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmountTmp;
    [SerializeField] private TextMeshProUGUI itemNameTmp;
    [SerializeField] private TextMeshProUGUI itemDescTmp;

    [SerializeField] private Button closeButton;

    public Button CloseButton { get => closeButton; }

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
}
