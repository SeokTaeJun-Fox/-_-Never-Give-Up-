using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//아이템 드롭 시스템의 추상 수신기 클래스입니다.
//드롭된 아이템을 처리하는 기본 인터페이스 역할을 하며, 하위 클래스에서 조건별로 구현됩니다.
public abstract class ItemDropReceiver : MonoBehaviour
{
    //드롭된 아이템을 처리합니다.
    //하위 클래스에서 조건에 따라 이벤트를 발생시키거나 아이템을 교환합니다.
    public abstract void DropItem(Item _item);
}
