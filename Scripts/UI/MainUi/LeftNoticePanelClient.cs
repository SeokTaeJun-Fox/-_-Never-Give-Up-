using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//좌측 알림 패널 사용자 클라이언트 클래스입니다.
public class LeftNoticePanelClient : MainUIElement
{
    [SerializeField] private LeftNoticePanel leftNoticePanel;

    private IItemManager itemManager;
    private bool isEventRegister;

    public override void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IItemManager item)
                itemManager = item;
        }
    }

    public override void Initial()
    {
        if (itemManager != null)
        {
            itemManager.OnGetItem += OnGetItem;
            itemManager.OnGetGold += OnGetGold;
            itemManager.OnGetItemFailed += OnGetItemFailed;
        }

        isEventRegister = true;
    }

    private void OnEnable()
    {
        if (isEventRegister)
            return;

        if (itemManager != null)
        {
            itemManager.OnGetItem += OnGetItem;
            itemManager.OnGetGold += OnGetGold;
            itemManager.OnGetItemFailed += OnGetItemFailed;

            isEventRegister = true;
        }
    }

    private void OnGetItemFailed(Item _item)
    {
        leftNoticePanel.ShowText($"<color=red>{_item.ItemName}</color>을/를 획득하는데 실패하였습니다.");
    }

    private void OnGetItem(Item _item, int _amount)
    {
        leftNoticePanel.ShowText($"<color=blue>{_item.ItemName}</color>을/를 획득하였습니다.");
    }

    private void OnGetGold(int _amount)
    {
        leftNoticePanel.ShowText($"<color=blue>{_amount}골드</color>를 획득하였습니다.");
    }

    private void OnDisable()
    {
        if (itemManager != null)
        {
            itemManager.OnGetItem -= OnGetItem;
            itemManager.OnGetGold -= OnGetGold;
            itemManager.OnGetItemFailed -= OnGetItemFailed;

            isEventRegister = false;
        }
    }

    private void OnDestroy()
    {
        if (itemManager != null)
        {
            itemManager.OnGetItem -= OnGetItem;
            itemManager.OnGetGold -= OnGetGold;
            itemManager.OnGetItemFailed -= OnGetItemFailed;

            isEventRegister = false;
        }
    }
}
