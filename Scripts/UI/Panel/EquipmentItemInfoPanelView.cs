using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EquipmentItemInfoPanelView : MonoBehaviour
{
    //상단 영역
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject equippedSignUi;
    [SerializeField] private TextMeshProUGUI itemNameTmp;
    [SerializeField] private TextMeshProUGUI categoryTmp;
    [SerializeField] private TextMeshProUGUI upgradeAmountTmp;

    [SerializeField] private List<EquipmentTypeAndStringInfoData> typeDatas;

    //가운데 영역
    [SerializeField] private Transform statPanelParent;
    [SerializeField] private ItemStatPanelView statPanelPreb;
    private List<ItemStatPanelView> statPrebCaches = new();

    //하단 영역
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private List<EquipmentStateInfoData> stateDatas;

    public Button EquipButton { get => equipButton; }
    public Button UnEquipButton { get => unEquipButton; }
    public Button CloseButton { get => closeButton; }

    //상단 세팅
    //아이템 이미지 보여줍니다.
    public void ShowItemImage(Sprite _itemSprite)
    { 
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = _itemSprite;
    }

    //착용 이미지 표시를 보여줍니다.
    public void ShowEquippedSignUi(bool _isOn)
    { 
        equippedSignUi.SetActive(_isOn);
    }

    //아이템 이름/카테고리를 보여줍니다.
    public void ShowItemTitle(string _itemName, EquipmentType _type, Color _itemNameColor)
    { 
        itemNameTmp.text = _itemName;
        itemNameTmp.color = _itemNameColor;
        categoryTmp.text = typeDatas.Find(x => x.type == _type).name;
    }

    public void ShowUpgradeAmount(int _upgradeAmount)
    {
        upgradeAmountTmp.text = _upgradeAmount.ToString();
    }

    //가운데 영역 세팅
    //아이템 스텟을 보여줍니다.
    public void ShowItemStat(List<ItemStatViewData> _datas)
    {
        int cacheIndex = 0;

        foreach (var data in _datas)
        {
            if (statPrebCaches.Count > cacheIndex)
            {
                statPrebCaches[cacheIndex].gameObject.SetActive(true);
                statPrebCaches[cacheIndex].ShowView(data.statCategory, data.content, data.color);
            }
            else
            {
                var statPanelView = Instantiate(statPanelPreb, statPanelParent);
                statPanelView.ShowView(data.statCategory, data.content, data.color);
                statPrebCaches.Add(statPanelView);
            }

            cacheIndex++;
        }
    }

    //하단 버튼 view 갱신
    public void ShowBottomButtons(EquipmentState _state)
    { 
        var stateData = stateDatas.Find(x => x.state == _state);
        equipButton.gameObject.SetActive(stateData.isEquipButtonOn);
        unEquipButton.gameObject.SetActive(stateData.isUnEquipButtonOn);
        closeButton.gameObject.SetActive(stateData.isCloseButtonOn);
    }

    //초기화
    public void Initial()
    {
        itemImage.gameObject.SetActive(false);
        itemImage.sprite = null;
        equippedSignUi.SetActive(false);

        foreach (var cachePanel in statPrebCaches)
        {
            cachePanel.Initial();
            cachePanel.gameObject.SetActive(false);
        }

        equipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
    }
}

[Serializable]
public struct ItemStatViewData
{
    public string statCategory;
    public string content;
    public Color color;
}

[Serializable]
public struct EquipmentTypeAndStringInfoData
{
    public EquipmentType type;
    public string name;
}

[Serializable]
public struct EquipmentStateInfoData
{
    public EquipmentState state;
    public bool isEquipButtonOn;
    public bool isUnEquipButtonOn;
    public bool isCloseButtonOn;
}
