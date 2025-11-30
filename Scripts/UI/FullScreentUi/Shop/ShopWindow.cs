using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopWindow : MonoBehaviour, IUIWindow
{
    [SerializeField] private BuyItemPanel buyItemPanel;
    [SerializeField] private SellItemPanel sellItemPanel;
    [SerializeField] private ShopOutPanelController shopOutPanelController;

    //private Action onClose;
    [SerializeField] private UnityEvent onClose;
    [SerializeField] private UnityEvent onOpen;

    //이 클래스에 필요한 스크립터블 인터페이스들
    private IEquipmentManager equipmentManager;
    private IItemManager itemManager;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IEquipmentManager equipmentManager)
                this.equipmentManager = equipmentManager;
            if (dependency is IItemManager itemManager)
                this.itemManager = itemManager;
        }
    }

    public void Initial(object[] _datas)
    {
        //if (_datas == null)
        //    return;

        //foreach (object data in _datas)
        //{
        //    if (data is Action closeEvent)
        //        onClose = closeEvent;
        //}
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        buyItemPanel.OpenPanel(itemManager);
        sellItemPanel.OpenPanel(itemManager, equipmentManager);
        shopOutPanelController.OpenPanel(itemManager, OnClose);
        onOpen?.Invoke();
    }

    public void OnClose()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }

    public void Close()
    {
        buyItemPanel.ClosePanel();
        sellItemPanel.ClosePanel();
        shopOutPanelController.ClosePanel();

        gameObject.SetActive(false);

        //onClose?.Invoke();
        onClose?.Invoke();
    }
}
