using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 상점에 보여주는 구매 아이템 목록들입니다.
[CreateAssetMenu(fileName = "BuyItemShopInfoRegistry", menuName = "Scriptable Object/Item/ETC/BuyItemShopInfoRegistry")]
public class BuyItemShopInfoRegistry : ScriptableObject
{
    [SerializeField] private List<BuyItemShopInfoEntry> buyItemInfoList;

    public List<BuyItemShopInfoEntry> BuyItemInfoList { get => buyItemInfoList; }
}
