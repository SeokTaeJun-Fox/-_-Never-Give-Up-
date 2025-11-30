using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalEnchantItemInfoPanelController : MonoBehaviour, IUIWindow
{
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 이벤트 스크립터블오브젝트
    [SerializeField] private ReportOpenWindowEvent openEvent;

    [SerializeField] private NormalEnchantItemInfoPanelView view;
    private ItemStack itemStack;

    private void Awake()
    {
        view.CloseButton.onClick.AddListener(OnClickCloseButton);
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        foreach (var data in _datas)
        {
            if (data is ItemStack stack)
            {
                itemStack = stack;
                view.SettingView(itemStack);
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);
    }

    public void Close()
    {
        view.ResetView();
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트
    private void OnItemPossessedChanged(IReadOnlyList<ItemStack> _itemStacks)
    {
        if (itemStack.amount == 0)
        {
            Close();
        }

        if (view != null)
            view.SettingView(itemStack);
    }

    private void OnClickCloseButton()
    {
        if (openEvent != null)
            openEvent.RaiseCloseWindow();

        Close();
    }
}
