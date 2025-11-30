using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 정보
[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Item/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemKey;       //아이템 객체 고유 키 (아이템 객체 마다 고유 id가 있습니다)
    [SerializeField] private int itemNum;       //아이템 번호 (아이템정보마다 고유 번호가 있습니다)
    [SerializeField] private string itemName;   //아이템명
    [SerializeField, TextArea] private string desc;       //설명
    [SerializeField] private Sprite icon;       //아이콘
    [SerializeField] private string objectPoolKey;     //해당 3d오브젝트 생성을 위한 오브젝트 풀 키값
    [SerializeField] private GameObject prefab; //item 3d오브젝트 (필드드랍용)
    [SerializeField] private bool isOverlap;    //아이템 겹치기 가능/불가능 (인벤토리에서 칸마다 겹치기 가능한지 확인합니다)
    [SerializeField] private int sellCost;  //판매 비용

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
