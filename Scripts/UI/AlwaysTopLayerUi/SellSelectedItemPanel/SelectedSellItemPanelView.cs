using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSellItemPanelView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI curPossessionItemAmountTmp;
    [SerializeField] private TextMeshProUGUI itemNameTmp;
    [SerializeField] private TextMeshProUGUI amountTmp;
    [SerializeField] private Button minButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private Slider amountSlider;

    [SerializeField] private Button sellButton;
    [SerializeField] private Button closeButton;

    public Button MinButton { get => minButton; }
    public Button MinusButton { get => minusButton; }
    public Button PlusButton { get => plusButton; }
    public Button MaxButton { get => maxButton; }
    public Slider AmountSlider { get => amountSlider; }
    public Button SellButton { get => sellButton; }
    public Button CloseButton { get => closeButton; }

    StringBuilder amountSb = new StringBuilder();

    public void SetImage(Sprite _spr)
    {
        itemImage.gameObject.SetActive(_spr != null);
        itemImage.sprite = _spr;
    }

    public void SetPossessionAmount(int _amount)
    {
        curPossessionItemAmountTmp.text = _amount.ToString();
    }

    public void SetItemName(string _name)
    {
        itemNameTmp.text = _name;
    }

    public void SetAmount(int _curAmount, int _maxAmount)
    {
        //슬라이더가 값을 변경할때마다 string연산을 하므로
        //최적화하기위해 스트링빌더를 사용했습니다.
        amountSb.Clear();
        amountSb.Append("수량설정 ");
        amountSb.Append(_curAmount);
        amountSb.Append("/");
        amountSb.Append(_maxAmount);

        amountTmp.text = amountSb.ToString();
    }
}
