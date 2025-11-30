using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : Damageable, ISkillUser, IItemUser, IBuffUser
{
    [Header("Animator")]
    [SerializeField] private string moveAnimationBoolName;  //움직임 체크 애니메이션 부울명
    [SerializeField] private string landAnimationBoolName;  //착지 체크 애니메이션 부울명
    [SerializeField] private string jumpAnimationTriggerName;   //점프 실행 애니메이션 트리거명
    [SerializeField] private string damageAnimationTriggerName; //피격 애니메이션 트리거명
    [SerializeField] private string damageActionEndBoolName;    //피격 종료 체크 애니메이션 부울명
    [SerializeField] private string animatorDieParamName;   //사망 애니메이션 트리거명

    [Header("Damage")]
    [SerializeField] private float damageActionTime;
    [SerializeField] private float remainDamageActionTime;
    public float RemainDamageActionTime
    {
        get => remainDamageActionTime;
        set => remainDamageActionTime = value;
    }

    [Header("Ability")]
    private IPlayerAbilityManager playerAbilityManager;
    private IReadOnlyDictionary<PlayerStat, object> playerAbilities;

    [Header("Dead")]
    [SerializeField] private string playerDeadLayerName;    //플레이어 사망 레이어명
    private bool isDead;

    [Header("Event ScriptableObject")]
    [SerializeField] private Weapon3DChangedEvent weapon3DChangedEvent;

    [Header("Debug")]
    [SerializeField] private bool isGizmoDebug;
    [SerializeField] private float debugRange;
    [SerializeField] private string debugParticlePoolName;
    [SerializeField] private string debugCode;

    [Header("SkillHotKey")]
    [SerializeField] private SkillHotKeyInfo skillHotKeyInfo;

    [Header("ETC")]
    [SerializeField] private Transform followObject;
    [SerializeField] private IParticleAttacher particleAttacher;

    [Header("SOUND")]
    [SerializeField] private CharacterSoundController characterSoundController;
    [SerializeField] private string jumpSfx;
    [SerializeField] private string damageSfx;
    [SerializeField] private string deadSfx;
    private bool canControl;

    private PlayerMovement playerMovement;
    private SkillController skillController;
    private PlayerRandomIdle playerRandomIdle;
    private PlayerItemPickUp playerItemPickUp;
    private PlayerFSM fsm;
    private IBuffController buffController;

    private AnimatorOverrideController ao;
    private Animator animator;

    public SkillController SkillController => skillController;
    public PlayerRandomIdle PlayerRandomIdle => playerRandomIdle;
    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerItemPickUp PlayerItemPickUp => playerItemPickUp;

    public Transform FollowObject => followObject;
    public bool CanControl => canControl;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        //애니메이터 초기화
        animator = GetComponent<Animator>();

        if (animator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController))
        {
            ao = (AnimatorOverrideController)animator.runtimeAnimatorController;
        }
        else
        {
            ao = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = ao;
        }

        //랜덤아이들
        playerRandomIdle = GetComponent<PlayerRandomIdle>();

        //스킬 컨트롤러
        skillController = GetComponent<SkillController>();
        if (skillController != null)
        {
            skillController.OnNormalAttackEnd += OnNormalAttackEnd;
            skillController.OnNormalAttackStart += OnNormalAttackStart;
        }

        if (playerMovement != null)
        {
            playerMovement.OnMoveEnter += OnMoveEnter;
            playerMovement.OnStop += OnStop;
            playerMovement.OnJump += OnJump;
            playerMovement.OnLand += OnLand;
        }

        //플레이어 아이템 픽업
        playerItemPickUp = GetComponent<PlayerItemPickUp>();

        //fsm 초기화
        fsm = GetComponent<PlayerFSM>();
        fsm.InitState(this, PlayerReadyState.Instance);
    }

    private void Update()
    {
        if (skillHotKeyInfo != null)
        {
            foreach (var info in skillHotKeyInfo.Infos)
            {
                if (Input.GetKeyDown(info.key) && info.skill != null)
                {
                    SkillController.CastSkill(info.skill);
                }
            }
        }
    }

    //버프 컨트롤러 세팅
    public void SetBuffController(IBuffController _buffController)
    { 
        buffController = _buffController;
    }

    //파티클 부착자세팅
    public void setOwnerParticleAttacher(IParticleAttacher _particleAttacher)
    { 
        particleAttacher = _particleAttacher;
    }

    //플레이어 조종을 시작한다
    public void StartControl()
    {
        fsm.InitState(this, PlayerNormalState.Instance);
    }

    //초기화
    public void InitialPlayer(IPlayerAbilityManager _playerAbilityManager, bool _canControl = true)
    {
        isDead = false;
        StartControl();
        playerMovement.SetCanMoveAndJump(_canControl); //플레이어 이동,점프가능 초기화
        canControl = _canControl;
        playerAbilities = _playerAbilityManager.PlayerAbilities;//playerAbilityManager.PlayerAbilities;
        playerAbilityManager = _playerAbilityManager;

        if (skillController != null)
        {
            skillController.Initial(this, null, 
                (_mp) => 
                {
                    _playerAbilityManager.IncreaseMp(_mp); 
                });

            skillController.OnCastSkill += OnCastSkill;
            skillController.OnCanCastSkillEnter += OnCanCastSkillEnter;
        }
        else
            Debug.LogError("PlayerController에 skillController가 존재하지않아 초기화하지 못했습니다.");

        weapon3DChangedEvent.OnWeaponChanged += On3DWeaponChanged;
    }

    //스킬 사용
    

    //상태 변경
    public void ChangePlayerState(IFSMState<PlayerController> _State)
    {
        if (fsm != null && _State != null)
            fsm.ChangeState(_State);
    }

    //피격
    public override void Damage(int _damage, bool _isDamageAction = true, bool _isIgnoreDef = false)
    {
        if (_isDamageAction)
        {
            PlayDamageAnimation(true);
            characterSoundController.PlayOneShotSFX(damageSfx);
            remainDamageActionTime = damageActionTime;
        }

        //체력 계산
        int damageAmount;

        if (_isIgnoreDef)
        {
            damageAmount = _damage;
        }
        else
        {
            damageAmount = _damage - (int)playerAbilities[PlayerStat.TOTAL_DEF];
        }

        damageAmount = Mathf.Clamp(damageAmount, 1, 999999);

        playerAbilityManager.IncreaseHp(-damageAmount);
        RaiseDamageEvent(damageAmount);
        Debug.Log($"player hp : {(int)playerAbilities[PlayerStat.HP]}");

        if ((int)playerAbilities[PlayerStat.HP] <= 0) //플레이어가 체력이 없을 때
        {
            Dead();
        }
        else if (!fsm.ToString().Contains("Damage") && _isDamageAction)    //플레이어가 체력이 있고 피격상태가 아닐 때
            ChangePlayerState(PlayerDamagedState.Instance); //피격 상태가 됩니다.
    }

    public void PlayDamageAnimation(bool _isPlayDamageAnimation) //피격 애니메이션 실행
    {
        if (_isPlayDamageAnimation)
        {
            Utility.ResetAllTrigger(animator);
            animator.SetTrigger(damageAnimationTriggerName);
            animator.SetBool(damageActionEndBoolName, false);
        }
        else
        {
            animator.SetBool(damageActionEndBoolName, true);
        }
    }

    public override void Addforce(Vector3 _force)
    {
        if(playerMovement != null) playerMovement.ApplyForce(_force);
    }

    public override void Dead()
    {
        isDead = true;
        OnDead.Invoke();
        Utility.ResetAllTrigger(animator);  //트리거 초기화
        animator.SetTrigger(animatorDieParamName);  //사망 애니메이션 실행을 위해 사망 트리거 실행
        gameObject.layer = LayerMask.NameToLayer(playerDeadLayerName);  //레이어설정 (사망레이어)
        ChangePlayerState(PlayerDeadState.Instance);    //사망 상태설정
        characterSoundController.PlayOneShotSFX(deadSfx);
    }

    public void SetCanControl(bool _canControl)
    {
        if (gameObject.layer == LayerMask.NameToLayer(playerDeadLayerName))
            return;

        if (_canControl)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");//레이어 설정
            canControl = true;
            
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("CantControlPlayer");//레이어 설정
            canControl = false;
        }
    }

    //이벤트
    private void OnJump()
    {
        Utility.ResetAllTrigger(animator);
        animator.SetBool(landAnimationBoolName, false);
        animator.SetTrigger(jumpAnimationTriggerName);
        characterSoundController.PlayOneShotSFX(jumpSfx);
    }

    private void OnMoveEnter(Vector3 pos)
    {
        animator.SetBool(moveAnimationBoolName, true);
    }

    private void OnStop()
    {
        animator.SetBool(moveAnimationBoolName, false);
    }

    private void OnLand()
    {
        animator.SetBool(landAnimationBoolName, true);
    }

    private void OnNormalAttackStart()
    {
        fsm.ChangeState(PlayerAttackState.Instance);
    }

    private void OnNormalAttackEnd()  //일반공격이 끝날때 실행합니다.
    {
        fsm.ChangeState(PlayerNormalState.Instance);
    }

    private void OnCanCastSkillEnter()  //스킬 시전을 할 수 있는 상태가 될 때 실행합니다.
    {
        fsm.ChangeState(PlayerNormalState.Instance);
    }

    private void OnCastSkill()  //스킬 시전하기 시작하면 실행합니다.
    {
        fsm.ChangeState(PlayerAttackState.Instance);
    }

    private void OnDrawGizmos()
    {
        if(isGizmoDebug)
        Gizmos.DrawSphere(transform.position, debugRange);
    }

    private void On3DWeaponChanged(GameObject _weapon)
    {
        skillController.SetWeaponTrail(_weapon.GetComponent<TrailRenderer>());
    }

    private void OnDestroy()
    {
        if (skillController != null)
        {
            skillController.OnNormalAttackEnd -= OnNormalAttackEnd;
            skillController.OnNormalAttackStart -= OnNormalAttackStart;
            skillController.OnCastSkill -= OnCastSkill;
            skillController.OnCanCastSkillEnter -= OnCanCastSkillEnter;
        }

        if (playerMovement != null)
        {
            playerMovement.OnJump -= OnJump;
            playerMovement.OnMoveEnter -= OnMoveEnter;
            playerMovement.OnStop -= OnStop;
            playerMovement.OnLand -= OnLand;
        }

        weapon3DChangedEvent.OnWeaponChanged -= On3DWeaponChanged;

        if (buffController != null)
            buffController.Stop();
    }

    #region ISkillUser
    public Transform Owner()
    {
        return transform;
    }

    public IReadOnlyDictionary<PlayerStat, object> Ability()
    {
        return playerAbilities;
    }

    public override void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        if (buffController != null && !isDead)
        { 
            buffController.GetBuff(_buff, _providerAbility, characterSoundController);
            if(_buff.Sfx != string.Empty)
                characterSoundController.PlayOneShotSFX(_buff.Sfx);
        }
        //Debug.Log($"{_buff.BuffCode} : buff take");
    }
    #endregion

    public override void ClearBuff(List<Category> _categories)
    {
        if (buffController != null)
        { 
            buffController.ClearBuff(_categories);
        }
    }

    #region IBuffUser
    public override void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale)
    {
        particleAttacher?.PlayParticleOneShot(_particlePoolName, _localPos, _isCancelable, _masterScale);
    }

    public override void PlayParticleOn(string _particlePoolName, string _code, float _masterScale)
    {
        particleAttacher?.PlayParticleOn(_particlePoolName, _code, _masterScale);
    }

    public override void PlayParticleOff(string _code)
    {
        particleAttacher?.PlayParticleOff(_code);
    }
    #endregion

    #region ItemUser
    public IReadOnlyDictionary<PlayerStat, object> PlayerAbilities => playerAbilities;

    public void IncreaseHp(int _amount)
    {
        if(playerAbilityManager != null)
            playerAbilityManager.IncreaseHp(_amount);
    }

    public void IncreaseMp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseMp(_amount);
    }

    public void IncreaseAddMaxHp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddMaxHp(_amount);
    }

    public void IncreaseAddMaxMp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddMaxMp(_amount);
    }

    public void IncreaseAddAtk(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddAtk(_amount);
    }

    public void IncreaseAddDef(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddDef(_amount);
    }

    public void ChangeAttackRange(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.ChangeAttackRange(_amount);
    }
    #endregion
}
