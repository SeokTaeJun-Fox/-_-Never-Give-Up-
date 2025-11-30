using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopOutPanelController : MonoBehaviour
{
    [SerializeField] private ShopOutPanelView view;

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    bool isOpenFirst = true;

    public void OpenPanel(IItemManager _itemManager, UnityAction onClose)
    { 
        itemManager = _itemManager;

        if (isOpenFirst)
        {
            view.CloseButton.onClick.AddListener(onClose);
        }

        if (itemManager != null)
        {
            view.SetGoldAmount(itemManager.GoldAmount);
            itemManager.OnGoldAmountChanged += OnGlodAmountChanged;
        }

        isOpenFirst = false;
    }

    public void ClosePanel()
    {
        if (itemManager != null)
        {
            itemManager.OnGoldAmountChanged -= OnGlodAmountChanged;
        }
    }

    //이벤트
    private void OnGlodAmountChanged(int _amount)
    {
        view.SetGoldAmount(_amount);
    }

    private void OnDestroy()
    {
        if (itemManager != null)
        {
            itemManager.OnGoldAmountChanged -= OnGlodAmountChanged;
        }
    }
}
