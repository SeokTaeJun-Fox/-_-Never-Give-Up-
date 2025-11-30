using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIAndUIWindowController : MonoBehaviour
{
    [SerializeField] private ActiveMainUIRequestEvent activeMainUIRequestEvent;
    [SerializeField] private OnActiveWindowCountChangedEvent activeWindowCountChangedEvent;

    private void Awake()
    {
        activeWindowCountChangedEvent.OnActiveWindowCountChanged += ChangeWindowCountLogic;
    }

    private void ChangeWindowCountLogic(int _count)
    {
        Debug.Log($"{_count} : ĭ");
        if (_count == 0)
        {
            activeMainUIRequestEvent.Raise(true);
        }
        else
        {
            activeMainUIRequestEvent.Raise(false);
        }
    }

    private void OnDestroy()
    {
        if(activeMainUIRequestEvent != null)
            activeWindowCountChangedEvent.OnActiveWindowCountChanged -= ChangeWindowCountLogic;
    }
}
