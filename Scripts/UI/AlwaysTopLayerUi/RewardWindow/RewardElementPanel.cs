using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RewardElementPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountTmp;
    [SerializeField] private TextMeshProUGUI itemNameTmp; 
    private Tween scaleTween;

    private void Awake()
    {
        scaleTween = transform.DOScale(Vector2.one, 0.5f).From(new Vector2(0.5f, 0.5f)).SetEase(Ease.OutElastic);
        scaleTween.SetAutoKill(false);
        scaleTween.Pause();
    }

    private void OnEnable()
    {
        scaleTween.Restart();
    }

    public void Setting(Sprite _itemSpr, string _amount, string _itemName)
    { 
        itemImage.sprite = _itemSpr;
        amountTmp.text = _amount;
        itemNameTmp.text = _itemName;
    }

    public void Setting(Reward _reward)
    {
        itemImage.sprite = _reward.Icon;
        amountTmp.text = _reward.Quantity.ToString();
        itemNameTmp.text = _reward.Description;
    }
}
