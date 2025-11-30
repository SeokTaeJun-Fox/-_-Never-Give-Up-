using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class MonsterController : Damageable, ISkillUser, IBuffUser
{
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody myRigid;
    private RuntimeAnimatorController ao;
    Collider collider;
    private IReadOnlyDictionary<PlayerStat, object> curMonsterAbility;   //현재 몬스터 능력치/상태
    private MonsterAbilityController monsterAbilityController;
    private MonsterFSM fsm;
    private SkillController skillController;
    private PatrolFunction patrolFunction;
    private MonsterChaseAttackController chaseAttackController;

    public Animator Animator => animator;
    public Rigidbody MyRigid => myRigid;
    public PatrolFunction PatrolFunction => patrolFunction;
    public MonsterChaseAttackController ChaseAttackController => chaseAttackController;

    [SerializeField] private MonsterInfoUser[] monsterInfoUsers;
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private string monsterName;    //몬스터 이름(몬스터 종류마다 다릅니다) 오브젝트 풀에 사용됩니다.

    public string MonsterName => monsterName;

    //해당 애니메이터 파라미터명
    [SerializeField] private string animatorIsWalkParamName;
    [SerializeField] private string animatorIsRunParamName;         //삭제예정
    [SerializeField] private string animatorIsBattleParamName;      //삭제예정
    [SerializeField] private string animatorAttackParamName;        //삭제예정
    [SerializeField] private string animatorDamageName;
    [SerializeField] private string animatorDamageEndName;
    [SerializeField] private string animatorDieParamName;

    //추격 (전체 삭제예정)
    [SerializeField] private float sightDegree; //시야각
    [SerializeField] private float sightDistance;   //시야범위
    [SerializeField] private float listenDistance;  //청력범위 (플레이어 추격에 사용됩니다)
    [SerializeField] private LayerMask chaseTargetLayer;    //추격 타겟
    [SerializeField] private LayerMask obstarcleTargetLayer;    //장애물 타겟
    [SerializeField] private string deadPlayerLayer;    //사망 플레이어 레이어 (플레이어 사망 확인시 필요합니다.)
    [SerializeField] private float chaseStopDistance;   //추격 목적지 오프셋
    [SerializeField] private float chaseSpeed;  //추격 속도

    [Header("SOUND")]
    [SerializeField] private CharacterSoundController characterSoundController;
    [SerializeField] private string damageSfx;

    #region 이부분삭제예정
    private Transform chaseTarget = null;
    public Transform ChaseTarget => chaseTarget;
    public bool IsChaseTargetDead
    {
        get
        {
            if (chaseTarget == null || chaseTarget.gameObject.layer == LayerMask.NameToLayer(deadPlayerLayer))
            {
                return true;
            }
            else
                return false;
        }
    }

    //공격
    [SerializeField] private float maxAttackDistance;   //최대 공격 거리
    [SerializeField] private Skill[] skillPattern;    //스킬패턴 (인덱스 0번부터 순서대로 공격합니다)
    //private int attackIndex;    //공격할 스킬 인덱스 번호

    public float MaxAttackDistance => maxAttackDistance;
    #endregion

    //피격
    [SerializeField] private float damageActionTime;    //피격 시간
    public float DamageActionTime => damageActionTime;

    private float remainDamageActionTime;
    public float RemainDamageActionTime
    { 
        get => remainDamageActionTime;
        set => remainDamageActionTime = value;
    }

    //사망
    [SerializeField] private UnityEvent onDead;
    private bool isDead;
    private UnityEvent<MonsterController> onDeadIncludeParam = new UnityEvent<MonsterController>();

    public event UnityAction<MonsterController> OnDeadIncludeParam
    {
        add => onDeadIncludeParam.AddListener(value);
        remove => onDeadIncludeParam.RemoveListener(value);
    }

    public NavMeshAgent Agent => agent;

    //디버그
    Collider[] targetCheckColliders = new Collider[1];

    //기타
    private int objectCategory;
    public int ObjectCategory { get => objectCategory; set => objectCategory = value; }
    private IParticleAttacher particleAttacher;
    private IBuffController buffController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myRigid  = GetComponent<Rigidbody>();
        patrolFunction = GetComponent<PatrolFunction>();
        collider = GetComponent<Collider>();
        chaseAttackController = GetComponent<MonsterChaseAttackController>();

        if (animator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController))
        {
            ao = (AnimatorOverrideController)animator.runtimeAnimatorController;
        }
        else
        {
            ao = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = ao;
        }

        monsterAbilityController = GetComponent<MonsterAbilityController>();
        fsm = GetComponent<MonsterFSM>();
        skillController = GetComponent<SkillController>();
    }

    //파티클 부착자세팅
    public void setOwnerParticleAttacher(IParticleAttacher _particleAttacher)
    {
        particleAttacher = _particleAttacher;
    }

    public void Setting(MonsterInfo _monsterInfo, Transform[] _patrolPos)
    {
        monsterAbilityController.Initial(_monsterInfo.MonsterAbility);
        curMonsterAbility = monsterAbilityController.MonsterAbilities;

        foreach (MonsterInfoUser _monsterInfoUser in monsterInfoUsers)
            _monsterInfoUser.Setting(_monsterInfo, curMonsterAbility);

        patrolFunction.Setting(_patrolPos);
        collider.enabled = true;
        agent.enabled = true;
        myRigid.isKinematic = true;
    }

    public void SetBuffController(IBuffController _buffController)
    { 
        buffController = _buffController;
    }

    //활동 시작
    public void ActiveStart()
    {
        isDead = false;
        //attackIndex = 0;
        skillController.Initial(this, null);
        skillController.SetActiveSkillController(false);
        fsm.InitState(this, MonsterNormalState.Instance);
    }

    //public void ChaseReset()
    //{
    //    agent.stoppingDistance = chaseStopDistance;
    //    agent.speed = chaseSpeed;
    //}

    /// <summary>
    /// 시야범위 혹은 청력범위내에 타겟이 있는지 확인합니다.
    /// </summary>
    /// <returns>현재 시야범위내에 타겟이 있으면 true로 반환됩니다.
    /// 또한 청력범위내에 타겟이 있으면 true로 반환됩니다.</returns>
    //public bool CheckFindTarget(out Transform target)
    //{
    //    //몬스터 창력 반경 [listenDistance]이내의 콜라이더들을 불러옵니다.
    //    int targetNum = Physics.OverlapSphereNonAlloc(transform.position, listenDistance, targetCheckColliders, chaseTargetLayer);
    //    if (targetNum > 0)
    //    {
    //        target = targetCheckColliders[0].transform;
    //        chaseTarget = target;
    //        return true;
    //    }

    //    //몬스터 반경 [sightDistance]이내의 콜라이더들을 불러옵니다.
    //    targetNum = Physics.OverlapSphereNonAlloc(transform.position, sightDistance, targetCheckColliders, chaseTargetLayer);

    //    //몬스터 반경 [sightDistance]이내 콜라이더가 있으면
    //    if (targetNum > 0)
    //    {
    //        //몬스터 시야각도 이내에 타겟이 있고, 몬스터와 타겟사이의 장애물이 없으면
    //        //몬스터가 타겟을 발견해 agent목적지는 타겟위치로 설정됩니다.
    //        Vector3 direction = targetCheckColliders[0].transform.position - transform.position;
    //        if (Utility.GetDegree(transform.forward, direction) <= sightDegree
    //            && !Physics.Raycast(transform.position, direction.normalized, direction.magnitude, obstarcleTargetLayer))
    //        {
    //            target = targetCheckColliders[0].transform;
    //            chaseTarget = target;
    //            return true;
    //        }
    //    }

    //    chaseTarget = null;
    //    target = null;
    //    return false;
    //}

    //public void PlayRunAnimation()
    //{
    //    animator.SetBool(animatorIsRunParamName, true);
    //    animator.SetBool(animatorIsWalkParamName, false);
    //    animator.SetBool(animatorIsBattleParamName, false);
    //}

    //공격
    //public void AttackStart()//공격시작
    //{
    //    animator.SetBool(animatorIsBattleParamName, true);
    //    skillController.SetActiveSkillController(true);
    //    skillController.OnCanCastSkillEnter += CheckCanAttackToTarget;

    //    Attack();
    //}

    //public void AttackStop()//공격종료
    //{
    //    animator.SetBool(animatorIsBattleParamName, false);
    //    skillController.SetActiveSkillController(false);
    //    skillController.OnCanCastSkillEnter -= CheckCanAttackToTarget;
    //}

    //public void Attack()
    //{
    //    if (chaseTarget != null)
    //    {
    //        transform.LookAt(chaseTarget);
    //        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    //    }

    //    if (skillController != null)
    //    {
    //        skillController.CastSkill(skillPattern[attackIndex]);
    //    }

    //    attackIndex++;
    //    if (attackIndex >= skillPattern.Length)
    //        attackIndex = 0;
    //}

    ////타겟을 대상으로 공격할수있는지 확인합니다.
    //private void CheckCanAttackToTarget()
    //{
    //    if (ChaseTarget != null)
    //    {
    //        //타겟이 사망하지않고, 타겟이 공격범위내에 있으면 공격합니다.
    //        if (!IsChaseTargetDead
    //         && Vector3.Distance(transform.position, ChaseTarget.position) <= Agent.stoppingDistance)
    //        {
    //            Attack();
    //            return;
    //        }

    //        //타겟이 공격범위내에 없고, 청각범위 혹은 시야범위내에 있으면 추격합니다.
    //        //그렇지 않으면 순찰합니다.
    //        Transform target = null;
    //        if (CheckFindTarget(out target))
    //        {
    //            ChangeMonsterState(MonsterChaseState.Instance);
    //        }
    //        else
    //        {
    //            ChangeMonsterState(MonsterNormalState.Instance);
    //        }
    //    }
    //}

    //피격
    public void PlayDamageAnimation(bool _isPlayDamageAnimation) //피격 애니메이션 실행/실행 해제
    {
        if (_isPlayDamageAnimation)
        {
            animator.SetBool(animatorDamageEndName, false);
            animator.SetTrigger(animatorDamageName);
        }
        else
        {
            animator.SetBool(animatorDamageEndName, true);
        }
    }

    //기타
    //상태 변경
    public void ChangeMonsterState(IFSMState<MonsterController> _State)
    {
        if (fsm != null && _State != null)
            fsm.ChangeState(_State);
    }

    private void ResetAllTrigger()
    {
        AnimatorControllerParameter[] triggerParams = animator.parameters.Where(acp => acp.type == AnimatorControllerParameterType.Trigger).ToArray();
        foreach (AnimatorControllerParameter triggerParam in triggerParams)
        {
            animator.ResetTrigger(triggerParam.name);
        }
    }


    public override void Addforce(Vector3 _force)
    {
        myRigid.AddForce(_force);
    }

    public override void Damage(int _damage, bool _isDamageAction = true, bool _isIgnoreDef = false)
    {
        if (_isDamageAction)
        {
            PlayDamageAnimation(true);
            remainDamageActionTime = damageActionTime;

            if(damageSfx != string.Empty)
                characterSoundController.PlayOneShotSFX(damageSfx);
        }

        if(!fsm.ToString().Contains("Damage") && _isDamageAction)
            ChangeMonsterState(MonsterDamagedState.Instance);

        //체력 계산
        int damageAmount;
        if (_isIgnoreDef)
        {
            damageAmount = _damage;
        }
        else
        {
            damageAmount = _damage - (int)curMonsterAbility[PlayerStat.TOTAL_DEF];
        }

        damageAmount = Mathf.Clamp(damageAmount, 1, 999999);
        monsterAbilityController.IncreaseHp(-damageAmount);
        RaiseDamageEvent(damageAmount);

        if ((int)curMonsterAbility[PlayerStat.HP] <= 0)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        isDead = true;
        CancelAllCancelableParticle();
        ResetAllTrigger();
        animator.SetTrigger(animatorDieParamName);
        ChangeMonsterState(MonsterDeadState.Instance);
        Agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        StartCoroutine(Destroy(3f)); //3초뒤 제거
        onDead?.Invoke();
        onDeadIncludeParam?.Invoke(this);
    }

    private IEnumerator Destroy(float _time)
    {
        yield return new WaitForSeconds(_time);
        DestroyImmediately();
    }

    private void DestroyImmediately()
    {
        poolEvent.RaiseRelease(monsterName, gameObject);
    }

    //ISkillUser
    public Transform Owner()
    {
        return transform;
    }

    public IReadOnlyDictionary<PlayerStat, object> Ability()
    {
        return curMonsterAbility;
    }

    public override void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        if (buffController != null && !isDead)
        { 
            buffController.GetBuff(_buff, _providerAbility, characterSoundController);
        }
    }

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

    public void CancelAllCancelableParticle()
    { 
        particleAttacher?.RemoveAllCancelableParticle();
    }

    //IBuffUser
    public void IncreaseHp(int _amount)
    {

    }

    public void IncreaseAddAtk(int _amount)
    {

    }

    public void IncreaseAddDef(int _amount)
    {

    }
    #endregion
}
