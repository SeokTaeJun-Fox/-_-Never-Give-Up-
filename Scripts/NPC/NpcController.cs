using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private PlayerSensor sensor;
    [SerializeField] private NpcCanvas npcCanvas;

    [SerializeField] private string talkAniParam;

    private Animator animator;
    private Transform targetPlayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (sensor != null)
        {
            sensor.OnEnter += OnEnterPlayer;
            sensor.OnExit += OnExitPlayer;
        }
    }

    private void Update()
    {
        if (targetPlayer != null)
        { 
            transform.LookAt(targetPlayer.position);
            transform.eulerAngles = Vector3.Scale(transform.eulerAngles, new Vector3(0, 1, 0));
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
        targetPlayer = _player;

        animator.SetBool(talkAniParam, true);

        if (npcCanvas != null)
            npcCanvas.ActivePressZImg(true);
    }

    private void OnExitPlayer()
    {
        targetPlayer = null;

        animator.SetBool(talkAniParam, false);

        if (npcCanvas != null)
            npcCanvas.ActivePressZImg(false);
    }
}
