using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class InventoryElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemButton;
    [SerializeField] private TextMeshProUGUI itemAmountTmp;
    [SerializeField] private GameObject equipSignUI;

    //외부
    [SerializeField] private ItemDragHandler itemDragHandler;
    [SerializeField] private InvenItemDropReceiver itemDropReceiver;

    public event Action<ItemStack> OnClickButton;
    public event Action<Item, Item> OnDropped;

    private ItemStack itemStack;

    private void Awake()
    {
        itemButton.onClick.AddListener(OnClick);

        if (itemDropReceiver != null)
            itemDropReceiver.OnDropSuccessed += OnDroppedItem;
    }

    public void Setting(ItemStack _itemStack)
    {
        //if (itemStack == null || !(itemStack.item.ItemKey.Equals(_itemStack.item.ItemKey)))
        //{
        //    itemImage.rectTransform.localScale = Vector3.zero;
        //    itemImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
        //}

        itemStack = _itemStack;
        itemImage.sprite = _itemStack.item.Icon;
        itemImage.gameObject.SetActive(true);

        if (_itemStack.item.IsOverlap)
            itemAmountTmp.text = _itemStack.amount.ToString();
        else
            itemAmountTmp.text = null;

        itemDragHandler?.Setting(_itemStack.item);

        if (itemDropReceiver != null)
            itemDropReceiver.myItem = itemStack?.item;
    }

    public void EquipSignUpdate()
    { 
        EquipmentItem equipmentItem = itemStack?.item as EquipmentItem;
        if (equipmentItem != null)
        {
            equipSignUI.SetActive(equipmentItem.IsEquipped);
        }
        else
        {
            equipSignUI.SetActive(false);
        }
    }

    public void Clear()
    {
        itemStack = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemAmountTmp.text = null;
        equipSignUI.SetActive(false);

        itemDragHandler?.Setting(null);

        if (itemDropReceiver != null)
            itemDropReceiver.myItem = null;
    }

    //버튼 클릭시 실행 버튼 OnClick()(인스펙터에 등록)
    public void OnClick()
    {
        OnClickButton?.Invoke(itemStack);
    }

    //인벤토리 칸에 아이템을 성공적으로 드랍할시 실행됩니다.
    private void OnDroppedItem(Item _item)
    {
        if(itemStack == null) return;

        OnDropped?.Invoke(itemStack.item, _item);
    }

    private void OnDestroy()
    {
        if(itemDropReceiver != null)
            itemDropReceiver.OnDropSuccessed -= OnDroppedItem;
    }
}
