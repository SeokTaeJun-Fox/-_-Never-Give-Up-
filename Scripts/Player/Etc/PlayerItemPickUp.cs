using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//플레이어가 아이템에 접근시 아이템을 얻을 수 있게 끔하는 클래스입니다.
public class PlayerItemPickUp : MonoBehaviour
{
    [Header("PickUpItem")]
    [SerializeField] private ItemProximityEvent itemProximityEvent;
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private List<Item3D> nearbyItems;
    [SerializeField] private Transform pickupItemPos;

    [SerializeField] private UnityEvent OnPickUpItem;

    private void AddItem(Item3D _item) => nearbyItems.Add(_item);
    private void RemoveItem(Item3D _item) => nearbyItems.Remove(_item);

    public void PickUpItem()
    {
        if (nearbyItems.Count != 0)
        {
            Item3D curPickUpItem = nearbyItems[0];
            curPickUpItem.GetComponent<Rigidbody>().useGravity = false;
            nearbyItems[0].transform.DOMove(pickupItemPos.position, 0.3f).OnComplete(() =>
            {
                curPickUpItem.PickUp();

                if (poolEvent != null)
                    poolEvent.RaiseRelease(curPickUpItem.ItemInfo.ObjectPoolKey, curPickUpItem.gameObject);
                //GameObject.Destroy(curPickUpItem.gameObject);
            });

            nearbyItems.RemoveAt(0);

            OnPickUpItem?.Invoke();
        }
    }

    private void OnEnable()
    {
        if (itemProximityEvent != null)
        {
            itemProximityEvent.OnEnter += AddItem;
            itemProximityEvent.OnExit += RemoveItem;
        }
    }

    private void OnDisable()
    {
        if (itemProximityEvent != null)
        {
            itemProximityEvent.OnEnter -= AddItem;
            itemProximityEvent.OnExit -= RemoveItem;
        }
    }
}
