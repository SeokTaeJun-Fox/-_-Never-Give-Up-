using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHotKeyPanelElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountTmp;
    [SerializeField] private Button button;

    public Button GetButton => button;
    private ConsumableItem target;

    public ConsumableItem TargetItem => target;

    public void ShowView(ConsumableItem _item, int _amount)
    {
        if (_item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = _item.Icon;
            amountTmp.text = _amount.ToString();
        }
        else
        {
            Hide();
        }

        target = _item;
    }

    public void Hide()
    {
        itemImage.enabled = false;
        amountTmp.text = "";
        target = null;
    }
}
