using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectedSellItemPanelController : MonoBehaviour, IUIWindow
{
    [SerializeField] private SelectedSellItemPanelModel model;
    [SerializeField] private SelectedSellItemPanelView view;

    //팝업
    [SerializeField] private string sellCompletePopupTitle;
    [SerializeField] private string sellCompletePopupContent;
    [SerializeField] private string sellCompleteYesText;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;
    [SerializeField] private UnityEvent onSell;      //아이템 구매할때 인스펙터에서도 작업이 가능합니다.

    //이 클래스에 필요한 인터페이스들
    private IItemManager itemManager;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;
    [SerializeField] private RequestOpenPopupWindowEvent requestPopupWindowEvent;

    private void Awake()
    {
        view.SellButton.onClick.AddListener(OnClickSellButton);
        view.CloseButton.onClick.AddListener(OnClickExitButton);
        view.AmountSlider.onValueChanged.AddListener(OnValueChangedAmountSlider);

        view.MinButton.onClick.AddListener(OnClickMinButton);
        view.MinusButton.onClick.AddListener(OnClickMinusButton);
        view.PlusButton.onClick.AddListener(OnClickPlusButton);
        view.MaxButton.onClick.AddListener(OnClickMaxButton);

        model.OnCurAmountChanged += OnCurAmountChanged;
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
        if (_datas == null)
            return;

        foreach (object data in _datas)
        {
            if (data is ItemStack itemstack)
            {
                if (itemManager != null)
                    Setting(itemstack);
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

    private void Setting(ItemStack _itemStack)
    {
        if (_itemStack == null)
            return;

        //모델 세팅
        //if (_itemStack.item.IsOverlap)
        //    model.SetInfo(_itemStack, 1, itemManager.GoldAmount / _itemStack.BuyCost);
        //else
        model.SetInfo(_itemStack, 1);

        //기타 뷰세팅
        view.SetImage(model.TargetItem.Icon);
        view.SetPossessionAmount(_itemStack.amount);
        view.SetItemName(model.TargetItem.ItemName);
        view.SellButton.interactable = true;

        //슬라이더 관련 세팅
        //view.SetAmount(model.CurAmount, model.MaxAmount);
        view.AmountSlider.minValue = 0;
        view.AmountSlider.maxValue = model.MaxAmount;
        view.AmountSlider.value = model.CurAmount;
    }

    private void SetSliderValue(float _value)
    {
        model.SetCurAmount((int)_value);
    }

    //이벤트
    private void OnValueChangedAmountSlider(float _value)
    {
        SetSliderValue(_value);
    }

    private void OnClickMinButton()
    {
        view.AmountSlider.value = 0;
    }

    private void OnClickMinusButton()
    {
        int amount = Mathf.Clamp(model.CurAmount - 1, 0, model.MaxAmount);
        view.AmountSlider.value = amount;
    }

    private void OnClickPlusButton()
    {
        int amount = Mathf.Clamp(model.CurAmount + 1, 0, model.MaxAmount);
        view.AmountSlider.value = amount;
    }

    private void OnClickMaxButton()
    {
        view.AmountSlider.value = model.MaxAmount;
    }

    //model데이터중 현재 슬라이더 값이 변경할때마다 이벤트 실행합니다.
    private void OnCurAmountChanged(int _amount)
    {
        view.SetAmount(_amount, model.MaxAmount);

        if (_amount == 0)
            view.SellButton.interactable = false;
        else
            view.SellButton.interactable = true;

        //슬라이더 변화 버튼 인터렉션 설정 
        view.MinButton.interactable = _amount != 0;
        view.MinusButton.interactable = _amount != 0;
        view.PlusButton.interactable = _amount != model.MaxAmount;
        view.MaxButton.interactable = _amount != model.MaxAmount;
    }

    private void OnClickSellButton()
    {
        if (itemManager == null)
            return;

        itemManager.RemoveItem(model.TargetItem, model.CurAmount);
        itemManager.GetGold(model.TargetItem.SellCost *  model.CurAmount);

        openEvent?.RaiseCloseWindow();
        onSell?.Invoke();
        Close();

        //팝업윈도우
        requestPopupWindowEvent.Raise(sellCompletePopupTitle, sellCompletePopupContent, false, sellCompleteYesText, null);
    }

    private void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }

    private void OnDestroy()
    {
        if (model != null)
            model.OnCurAmountChanged -= OnCurAmountChanged;
    }
}
