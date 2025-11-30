using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private PortalPanelData panelData;
    [SerializeField] private PlayerSensor sensor;
    [SerializeField] private NpcCanvas npcCanvas;

    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;
    [SerializeField] private LoadSceneRequestEvent loadSceneRequestEvent;

    private bool isEnter;

    private void Awake()
    {
        sensor.OnEnter += EnterPlayer;
        sensor.OnExit += ExitPlayer;
        npcCanvas.SettingName("Æ÷Å»");
    }

    private void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.Z))
        {
            if (panelData.Datas.Count > 1)
                factoryEvent.Raise(UIType.PORTAL_WINDOW, new object[] { panelData });
            else if (panelData.Datas.Count == 1)
                loadSceneRequestEvent.Raise(panelData.Datas[0].LoadSceneType, panelData.Datas[0].MapId);
        }
    }

    private void EnterPlayer(Transform _target)
    {
        isEnter = true;
        npcCanvas.ActivePressZImg(true);
    }

    private void ExitPlayer()
    {
        isEnter = false;
        npcCanvas.ActivePressZImg(false);
    }
}
