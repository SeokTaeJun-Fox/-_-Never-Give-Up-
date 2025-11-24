using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//인벤토리 UI에서 장비 슬롯의 시각적 표현을 담당하는 클래스입니다.
//장비 아이콘 표시, 장착/해제 애니메이션, 강화 효과 연출을 처리합니다.
public class InventoryEquipmentPanelView : MonoBehaviour
{
    [SerializeField] private List<EquipmentAndPanelInfo> equipmentAndPanelInfos;    //장비 슬롯 정보 리스트
    [SerializeField] private float AppearEffectTime;    //장착 시 연출 시간
    [SerializeField] private float DisAppearEffectTime; //해제 시 연출 시간
    [SerializeField] private float effectTime;  //강화 효과 연출 시간

    public IReadOnlyList<EquipmentAndPanelInfo> EquipmentAndPanelInfos => equipmentAndPanelInfos.AsReadOnly();
    private Sequence effectSequence;

    //특정 장비 슬롯에 아이콘을 표시하거나 숨깁니다.
    public void Show(EquipmentType _type, Sprite _spr)
    { 
        EquipmentAndPanelInfo info = equipmentAndPanelInfos.Find(x => x.type == _type);
        if (info != null)
        {
            //장비ui 보여주기,사라지기 연출
            if (_spr != null)
            {
                info.image.gameObject.SetActive(true);
                info.image.sprite = _spr;
                info.image.rectTransform.DOScale(Vector2.one, AppearEffectTime).From(Vector2.zero);
            }
            else
            {
                info.image.rectTransform.DOScale(Vector2.zero, DisAppearEffectTime).From(Vector2.one).OnComplete(() =>
                {
                    info.image.gameObject.SetActive(false);
                });
            }
        }
    }

    //모든 장비 슬롯을 초기화합니다.
    public void Initial()
    { 
        foreach(var element in equipmentAndPanelInfos)
            element.image.gameObject.SetActive(false);
    }

    //특정 장비 슬롯에 강화 효과를 연출합니다.
    public void Effect(EquipmentType _type)
    {
        EquipmentAndPanelInfo info = equipmentAndPanelInfos.Find(x => x.type == _type);
        if (info != null)
        {
            effectSequence = DOTween.Sequence();
            effectSequence.Append(info.effect.rectTransform.DOScale(Vector2.one * 5, effectTime).From(Vector2.zero))
            .Join(info.effect.DOFade(0, effectTime).From(1));
        }
    }
}

//장비 슬롯UI 요소를 묶는 구조체입니다.
[System.Serializable]
public class EquipmentAndPanelInfo
{
    public EquipmentType type;  //장비 타입
    public Image image; //장비 아이콘 이미지
    public Button button;   //정보 패널 열기 버튼
    public Image effect;    //강화 효과 이미지
}
