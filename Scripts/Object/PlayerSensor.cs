using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

//플레이어를 감지하는 클래스입니다.
public class PlayerSensor : MonoBehaviour
{
    public event Action<Transform> OnEnter;
    public event Action OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            (other.gameObject.layer == LayerMask.NameToLayer("CantControlPlayer")))
        {
            OnEnter?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
    (other.gameObject.layer == LayerMask.NameToLayer("CantControlPlayer")))
        {
            OnExit?.Invoke();
        }
    }
}
