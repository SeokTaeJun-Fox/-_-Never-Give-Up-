using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3d아이템을 컨트롤하는 클래스입니다.
public abstract class Item3D : MonoBehaviour
{
    protected int amount;
    [SerializeField] protected Item itemInfo; //아이템 정보

    public Item ItemInfo => itemInfo;

    //아이템을 획득시 실행합니다.
    public abstract void PickUp();

    public abstract void Setting(Item _item, int _amount);
}
