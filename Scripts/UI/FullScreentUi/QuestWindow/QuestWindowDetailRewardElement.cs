using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestWindowDetailRewardElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;

    public void Setting(Sprite _itemSpr, string _itemName)
    { 
        itemImage.sprite = _itemSpr;
        itemName.text = _itemName;
    }
}
