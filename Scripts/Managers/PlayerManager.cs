using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerManager : OnDamagedEventUser
{
    [SerializeField] private GameObject playerPreb;
    [SerializeField] private GameObject cameraPreb;

    [SerializeField] private ItemUserDeputy itemUserDeputy; //장비 아이템을 대신 착용하는 클래스 (능력치는 반영합니다)

    //스크립터블 오브젝트(이벤트)
    [SerializeField] private GetIItemUserEvent getIItemUserEvent;
    [SerializeField] private GetISkillUserEvent getISkillUserEvent;
    [SerializeField] private GetPlayerControllerEvent getPlayerControllerEvent;
    [SerializeField] private OnActiveWindowCountChangedEvent activeWindowCountChangedEvent;
    [SerializeField] private OnPlayerDamagedEvent onPlayerDamagedEvent; //플레이어에서 붙힌 데미지 이벤트와 비교했을 때
                                                                        //이 이벤트는
                                                                        //플레이어가 삭제하고 재 생성해도 피격 데미지 이벤트 연결은 유지됩니다.
    [SerializeField] private CreatePlayerRequestEvent createPlayerRequestEvent;

    [SerializeField] private UnityEvent OnPlayerDead;

    //인터페이스
    private IPlayerAbilityManager playerAbility;
    private IBuffController buffController;

    private bool canControlPlayer;
    private PlayerController playerController;

    private void Awake()
    {
        canControlPlayer = true;

        if(getIItemUserEvent != null)
            getIItemUserEvent.OnGet += GetIItemUser;

        if(getISkillUserEvent != null)
            getISkillUserEvent.OnGet += GetISkillUser;

        if (getPlayerControllerEvent != null)
            getPlayerControllerEvent.OnGet += GetPlayerContrller;

        if(activeWindowCountChangedEvent != null)
            activeWindowCountChangedEvent.OnActiveWindowCountChanged += OnActiveWindowCountChanged;

        if (createPlayerRequestEvent != null)
            createPlayerRequestEvent.OnCreatePlayerRequest += CreatePlayer;
    }

    private void Start()
    {
        playerAbility = ServiceLocator.GetService<IPlayerAbilityManager>();
        buffController = ServiceLocator.GetService<IBuffController>();

        itemUserDeputy.SetAbilityManager(playerAbility);
    }

    public PlayerController CreatePlayer(Vector3 _pos, Quaternion _rot)
    {
        //플레이어, 카메라객체 생성 및 플레이어 변수에 저장
        playerController = Instantiate(playerPreb, _pos, _rot).GetComponent<PlayerController>();
        GameObject cameraParent = Instantiate(cameraPreb);

        //플레이어 컨트롤러에 해당 인터페이스를 사용하기위해 인터페이스 주입 (IPlayerManager)
        //IPlayerAbilityManager playerAbility = ServiceLocator.GetService<IPlayerAbilityManager>();
        playerController.InitialPlayer(playerAbility, canControlPlayer);

        //플레이어 컨트롤러에 해당 인터페이스를 사용하기위해 인터페이스 주입 (IBuffController)
        //IBuffController buffController = ServiceLocator.GetService<IBuffController>();
        playerController.SetBuffController(buffController);
        buffController.SetOwner(playerController);

        buffController.Play();

        //플레이어 사망시 이벤트 실행
        playerController.OnDead.AddListener(buffController.ClearAllBuff);
        playerController.OnDead.AddListener(PlayPlayerDeadEvent);

        //카메라, 플레이어에 사용할 클래스 인스턴스 설정
        CinemachineCameraRotate ccRotate = cameraParent.GetComponent<CinemachineCameraRotate>();
        PlayerCinemachineCameraSetting pccSetting = cameraParent.GetComponent<PlayerCinemachineCameraSetting>();
        PlayerMovement pm = playerController.GetComponent<PlayerMovement>();

        //카메라 회전 담당 클래스, 시네머신세팅 클래스 에 플레이어를 따라가는 오브젝트 설정
        ccRotate.ObjectToFollow = playerController.FollowObject;
        pccSetting.Setting(playerController.FollowObject);
        
        //회전 이벤트 추가
        pm.OnRotate += ccRotate.OnRotate;
        pm.SetCamera = Camera.main;

        //데미지 이벤트 추가
        playerController.AddOnDamagedEventUser(this);

        return playerController;
    }

    private IItemUser GetIItemUser()
    {
        if (playerController != null)
            return playerController;
        else
            return itemUserDeputy;
    }

    private ISkillUser GetISkillUser()
    {
        return playerController;
    }

    private PlayerController GetPlayerContrller()
    {
        return playerController;
    }

    //이벤트
    private void OnDestroy()
    {
        if(getIItemUserEvent != null)
            getIItemUserEvent.OnGet -= GetIItemUser;

        if(getISkillUserEvent != null)
            getISkillUserEvent.OnGet -= GetISkillUser;

        if (getPlayerControllerEvent != null)
            getPlayerControllerEvent.OnGet -= GetPlayerContrller;

        if (activeWindowCountChangedEvent != null)
            activeWindowCountChangedEvent.OnActiveWindowCountChanged -= OnActiveWindowCountChanged;

        if (playerController != null)
        {
            playerController.RemoveOnDamagedEventUser(this);
            playerController.OnDead.RemoveListener(PlayPlayerDeadEvent);
        }

        if (createPlayerRequestEvent != null)
            createPlayerRequestEvent.OnCreatePlayerRequest -= CreatePlayer;
    }

    private void OnActiveWindowCountChanged(int _count)
    {
        if (_count > 0)
        {
            canControlPlayer = false;

            if(playerController != null)
                playerController?.SetCanControl(false);
        }
        else
        {
            canControlPlayer = true;

            if(playerController != null)
                playerController?.SetCanControl(true);
        }
    }

    public override void OnDamaged(int _damage)
    {
        onPlayerDamagedEvent?.RaiseEvent(_damage);
    }

    public void PlayPlayerDeadEvent()
    {
        OnPlayerDead?.Invoke();
    }
}
