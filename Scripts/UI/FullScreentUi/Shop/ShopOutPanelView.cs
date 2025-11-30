using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopOutPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmountTmp;
    [SerializeField] private Button closeButton;

    public Button CloseButton { get => closeButton; }

    public void SetGoldAmount(int _amount)
    { 
        goldAmountTmp.text = _amount.ToString();
    }
}
