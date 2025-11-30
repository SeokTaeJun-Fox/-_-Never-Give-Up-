using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//코인 아이템3d 처리 클래스입니다.
public class CoinItem3d : Item3D
{
    [SerializeField] private ItemProximityEvent itemProximityEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StaticConst.playerLayer))
        {
            if (itemProximityEvent != null)  itemProximityEvent.RaiseEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StaticConst.playerLayer))
        {
            if(itemProximityEvent != null) itemProximityEvent.RaiseExit(this);
        }
    }

    public override void PickUp()
    {
        Debug.Log($"{amount} 골드 get");
        //아이템 저장 매니저에게 요청
        ServiceLocator.GetService<IItemManager>().GetGold(amount);
    }

    public override void Setting(Item _item, int _amount)
    {
        itemInfo = _item;
        amount = _amount;
    }
}
