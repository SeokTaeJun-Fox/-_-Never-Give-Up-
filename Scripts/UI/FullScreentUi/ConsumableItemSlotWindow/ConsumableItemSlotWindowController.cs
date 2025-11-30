using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConsumableItemSlotWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private UnityEvent onSetItem;
    [SerializeField] private UnityEvent onSetCancelItem;
    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    //이 클래스에 필요한 스크립터블오브젝트들
    [SerializeField] private ConsumableItemHotKeyInfo keyInfo;

    [SerializeField] private ItemHotKeyPanelElement[] hotKeyPanels;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button uiBlockButton;

    private ConsumableItem target;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);
        uiBlockButton.onClick.AddListener(OnClickCloseButton);

        for (int i = 0; i < hotKeyPanels.Length; i++)
        {
            int index = i;
            hotKeyPanels[i].GetButton.onClick.AddListener(() =>
            {
                if (itemManager == null)
                    return;

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (keyInfo.GetItem(index) != null)
                        onSetCancelItem?.Invoke();

                    hotKeyPanels[index].Hide();
                    keyInfo?.SetItem(index, null);
                }
                else
                {
                    ItemStack itemStack = itemManager.GetItemStack(target);
                    if (itemStack == null)  //아이템매니저 보유항목에 없으면 아이템갯수는 0입니다.
                    {
                        hotKeyPanels[index].ShowView(target, 0);
                    }
                    else
                    {
                        hotKeyPanels[index].ShowView(target, itemStack.amount);
                    }

                    if (keyInfo.GetItem(index) == null || keyInfo.GetItem(index).ItemNum != target.ItemNum)
                        onSetItem.Invoke();

                    keyInfo?.SetItem(index, target);
                }
            });
        }
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IItemManager item)
                itemManager = item;
        }
    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is ConsumableItem item)
                target = item;
        }

        if (itemManager == null)
        {
            return;
        }

        for (int i = 0; i < hotKeyPanels.Length; i++)
        {
            ConsumableItem item = keyInfo.GetItem(i);
            
            ItemStack itemStack;
            if (item == null)
            {
                hotKeyPanels[i].Hide();
            }
            else
            {
                itemStack = itemManager.GetItemStack(item);
                if (itemStack == null)  //해당 아이템이 슬롯에 등록되었지만 아이템매니저 보유항목에 없으면 아이템갯수는 0입니다.
                {
                    hotKeyPanels[i].ShowView(item, 0);
                }
                else
                {
                    hotKeyPanels[i].ShowView(item, itemStack.amount);
                }
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트
    private void OnClickCloseButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
