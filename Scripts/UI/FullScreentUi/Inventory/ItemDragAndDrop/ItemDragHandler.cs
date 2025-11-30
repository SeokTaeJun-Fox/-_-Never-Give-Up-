using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//인벤토리 UI의 아이템 슬롯에 붙여있는 드래그 핸들러입니다.
//아이템 드래그 시 이미지 표시, 드롭 대상 감지 및 이벤트 전달을 처리합니다.
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image dragImage;   //드래그 중 표시될 아이템 이미지
    [SerializeField] private RectTransform dragImageRect; //드래그 이미지 위치
    private Item target;    //드래그 대상 아이템
    private bool isUsed;    //드래그 상태 여부

    [SerializeField] private UnityEvent onBeginDragSuccessed;   //드래그 시작 성공 이벤트
    [SerializeField] private UnityEvent onEndDragSuccessed; //드래그 종료 성공 이벤트

    //드래그 대상 아이템을 설정합니다.
    public void Setting(Item _item)
    { 
        target = _item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragImage != null && target != null)
        {
            dragImage.gameObject.SetActive(true);
            dragImage.sprite = target.Icon;

            UpdateDragPosition(eventData);
            isUsed = true;

            onBeginDragSuccessed?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragPosition(eventData);
    }

    //아이템을 놓을 때 ItemDropReceiver가 붙은 오브젝트에 놓으면 해당 드랍리시버의 DropItem메소드가 호출되고
    //드랍 성공 이벤트가 호출됩니다.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null && target != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                var dropTarget = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemDropReceiver>();
                if (dropTarget != null)
                {
                    dropTarget.DropItem(target);
                    onEndDragSuccessed?.Invoke();
                }
            }

            dragImage.gameObject.SetActive(false);
            dragImage.sprite = null;
            isUsed = false;
        }
    }

    //드래그 이미지의 위치를 갱신합니다.
    private void UpdateDragPosition(PointerEventData eventData)
    {
        if (dragImageRect != null)
            dragImageRect.position = eventData.position;
    }

    private void OnDisable()
    {
        if (isUsed && dragImage != null)
        {
            dragImage.gameObject.SetActive(false);
            dragImage.sprite = null;
            isUsed = false;
        }
    }
}
