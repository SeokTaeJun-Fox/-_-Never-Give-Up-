using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인벤토리 아이템 슬롯에 붙여있는 드롭 수신기입니다.
//아이템 간 교환 조건을 확인하고 이벤트를 발생시킵니다.
public class InvenItemDropReceiver : ItemDropReceiver
{
    //아이템 드롭 성공 이벤트
    public event Action<Item> OnDropSuccessed;

    //현재 슬롯에 등록된 아이템
    public Item myItem;

    //아이템 드롭 시 교환 조건을 확인하고 이벤트를 발생시킵니다.
    //조건은 이 이벤칸의 아이템이 존재하고
    //바꿀 아이템이 존재하고, 두 아이템이 서로 다르면 이벤트가 실행됩니다.
    public override void DropItem(Item _item)
    {
        if (myItem != null && _item != null && myItem.ItemKey != _item.ItemKey)
            OnDropSuccessed(_item);
    }
}
