using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralItem3d : Item3D
{
    [SerializeField] private ItemProximityEvent itemProximityEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StaticConst.playerLayer))
        {
            if (itemProximityEvent != null)  itemProximityEvent.RaiseEnter(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Rigidbody rigid = GetComponent<Rigidbody>();

        if (rigid != null)
        {
            rigid.useGravity = false;
            rigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StaticConst.playerLayer))
        {
            if (itemProximityEvent != null)  itemProximityEvent.RaiseExit(this);
        }
    }

    public override void PickUp()
    {
        Debug.Log($"{transform.name} : pick");

        ServiceLocator.GetService<IItemManager>().GetItem(ItemInfo, amount);
    }

    public override void Setting(Item _item, int _amount)
    {
        itemInfo = _item;
        amount = _amount;
    }
}
