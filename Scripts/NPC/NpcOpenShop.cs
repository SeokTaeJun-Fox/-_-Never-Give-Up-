using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcOpenShop : MonoBehaviour
{
    [SerializeField] private PlayerSensor sensor;
    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;

    private bool isInteraction = false;

    private void Awake()
    {
        if (sensor != null)
        {
            sensor.OnEnter += OnEnterPlayer;
            sensor.OnExit += OnExitPlayer;
        }
    }

    private void Update()
    {
        if (isInteraction && Input.GetKeyDown(KeyCode.Z))
        {
            factoryEvent.Raise(UIType.SHOP_WINDOW);
        }
    }

    //¿Ã∫•∆Æ
    private void OnDestroy()
    {
        if (sensor != null)
        {
            sensor.OnEnter -= OnEnterPlayer;
            sensor.OnExit -= OnExitPlayer;
        }
    }

    private void OnEnterPlayer(Transform _player)
    {
        isInteraction = true;
    }

    private void OnExitPlayer()
    {
        isInteraction = false;
    }
}
