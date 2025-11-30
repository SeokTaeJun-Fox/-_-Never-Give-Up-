using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 상점의 구매 아이템 목록 1개정보입니다.
[CreateAssetMenu(fileName = "BuyItemInfo_", menuName = "Scriptable Object/Item/ETC/BuyItemShopInfoEntry")]
public class BuyItemShopInfoEntry : ScriptableObject
{
    [SerializeField] private Item item; //아이템
    [SerializeField] private int buyCost;   //아이템의 구매 가격

    public Item Item { get => item; }
    public int BuyCost { get => buyCost; }
}
