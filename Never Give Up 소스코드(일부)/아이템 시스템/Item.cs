using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 내 아이템의 기본 정보를 담고 있는 스크립터블오브젝트입니다.
[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Item/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemKey;       //아이템 객체 고유 키 (인스턴스마다 유일)
    [SerializeField] private int itemNum;       //아이템 번호 (아이템 종류마다 고유)
    [SerializeField] private string itemName;   //아이템 이름
    [SerializeField, TextArea] private string desc;       //아이템 설명
    [SerializeField] private Sprite icon;       //아이템 아이콘 이미지
    [SerializeField] private string objectPoolKey;     //해당 3d오브젝트 생성을 위한 오브젝트 풀 키값
    [SerializeField] private GameObject prefab; //필드 드랍용 3D 오브젝트 프리팹
    [SerializeField] private bool isOverlap;    //인벤토리에서 중첩 가능 여부
    [SerializeField] private int sellCost;  //판매 가격

    //아이템 타입 (기본값은 NORMAL, 상속 시 오버라이드 가능)
    public virtual ItemType ItemType
    {
        get => ItemType.NORMAL;
    }

    public string ObjectPoolKey { get => objectPoolKey;}
    public string ItemKey { get => itemKey; set => itemKey = value; }
    public string ItemName { get => itemName;}
    public string Desc { get => desc; }
    public Sprite Icon { get => icon; }
    public GameObject Prefab { get => prefab; }
    public bool IsOverlap { get => isOverlap; }
    public int SellCost { get => sellCost; }
    public int ItemNum { get => itemNum; }
}
