using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터의 추격 및 공격 행동을 제어하는 클래스입니다.
//시야 및 청력 기반 타겟 탐지, 추격 이동, 공격 애니메이션 처리, 스킬 패턴 실행,
//상태 전환(FSM)까지 포함한 AI 전투 로직을 담당합니다.
public class MonsterChaseAttackController : MonoBehaviour
{
    //컴포넌트 참조
    private NavMeshAgent agent;
    private SkillController skillController;
    private Animator animator;
    private MonsterFSM fsm;

    //애니메이터 파라미터 이름
    [SerializeField] private string animatorIsBattleParamName;
    [SerializeField] private string animatorIsWalkParamName;
    [SerializeField] private string animatorIsRunParamName;

    //추격 관련 설정
    [SerializeField] private float sightDegree; //시야각도
    [SerializeField] private float sightDistance;   //시야거리
    [SerializeField] private float listenDistance;  //청력거리 (플레이어 추격에 사용됩니다)
    [SerializeField] private LayerMask chaseTargetLayer;    //추격 대상 레이어
    [SerializeField] private LayerMask obstarcleTargetLayer;    //장애물 레이어
    [SerializeField] private string deadPlayerLayer;    //사망 플레이어 레이어 (플레이어 사망 확인시 필요합니다.)
    [SerializeField] private float chaseStopDistance;   //추격 정지 거리
    [SerializeField] private float chaseSpeed;  //추격 속도

    private Transform chaseTarget = null;   //현재 추격 중인 대상
    public Transform ChaseTarget => chaseTarget;

    //추격 대상이 사망했는지 여부를 반환합니다.
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

    [SerializeField] private Skill[] skillPattern;    //스킬 패턴 배열 (인덱스 0번부터 순서대로 공격합니다)
    private Collider[] targetCheckColliders = new Collider[1];  //타겟 탐지용 배열
    private int attackIndex;    //현재 사용할 스킬 인덱스

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fsm = GetComponent<MonsterFSM>();
        skillController = GetComponent<SkillController>();
    }

    private void OnEnable()
    {
        attackIndex = 0;
    }

    //추격 관련 설정을 초기화합니다.
    public void ChaseReset()
    {
        agent.stoppingDistance = chaseStopDistance;
        agent.speed = chaseSpeed;
    }

    /// <summary>
    /// 시야 또는 청력 범위 내에 타겟이 있는지 확인합니다.
    /// </summary>
    /// <returns>현재 시야범위내에 타겟이 있으면 true로 반환됩니다.
    /// 또한 청력범위내에 타겟이 있으면 true로 반환됩니다.</returns>
    public bool CheckFindTarget(out Transform target)
    {
        //몬스터 창력 반경 [listenDistance]이내의 콜라이더들을 불러옵니다.
        int targetNum = Physics.OverlapSphereNonAlloc(transform.position, listenDistance, targetCheckColliders, chaseTargetLayer);
        if (targetNum > 0)
        {
            target = targetCheckColliders[0].transform;
            chaseTarget = target;
            return true;
        }

        //몬스터 반경 [sightDistance]이내의 콜라이더들을 불러옵니다.
        targetNum = Physics.OverlapSphereNonAlloc(transform.position, sightDistance, targetCheckColliders, chaseTargetLayer);

        //몬스터 반경 [sightDistance]이내 콜라이더가 있으면
        if (targetNum > 0)
        {
            //몬스터 시야각도 이내에 타겟이 있고, 몬스터와 타겟사이의 장애물이 없으면
            //몬스터가 타겟을 발견해 agent목적지는 타겟위치로 설정됩니다.
            Vector3 direction = targetCheckColliders[0].transform.position - transform.position;
            if (Utility.GetDegree(transform.forward, direction) <= sightDegree
                && !Physics.Raycast(transform.position, direction.normalized, direction.magnitude, obstarcleTargetLayer))
            {
                target = targetCheckColliders[0].transform;
                chaseTarget = target;
                return true;
            }
        }

        //탐지 실패
        chaseTarget = null;
        target = null;
        return false;
    }

    //공격을 시작합니다.
    //스킬을 사용할 수 있는 상태로 전활 할 때마다 CheckCanAttackToTarget메소드 실행합니다.
    public void AttackStart()
    {
        animator.SetBool(animatorIsBattleParamName, true);
        skillController.SetActiveSkillController(true);
        skillController.OnCanCastSkillEnter += CheckCanAttackToTarget;

        Attack();
    }

    //공격을 종료합니다.
    public void AttackStop()
    {
        animator.SetBool(animatorIsBattleParamName, false);
        skillController.SetActiveSkillController(false);
        skillController.OnCanCastSkillEnter -= CheckCanAttackToTarget;
    }

    //현재 타겟을 향해 회전 후 스킬을 시전합니다.
    public void Attack()
    {
        if (chaseTarget != null)
        {
            transform.LookAt(chaseTarget);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        if (skillController != null)
        {
            skillController.CastSkill(skillPattern[attackIndex]);
        }

        attackIndex++;
        if (attackIndex >= skillPattern.Length)
            attackIndex = 0;
    }

    //타겟이 공격 가능 상태인지 확인하고, 가능하면 공격합니다.
    //그렇지 않으면 상태를 추격 또는 순찰로 전환합니다.
    private void CheckCanAttackToTarget()
    {
        if (ChaseTarget != null)
        {
            //타겟이 사망하지않고, 타겟이 공격범위내에 있으면 공격합니다.
            if (!IsChaseTargetDead
             && Vector3.Distance(transform.position, ChaseTarget.position) <= agent.stoppingDistance)
            {
                Attack();
                return;
            }

            //타겟이 공격범위내에 없고, 청각범위 혹은 시야범위내에 있으면 추격합니다.
            //그렇지 않으면 순찰합니다.
            Transform target = null;
            if (CheckFindTarget(out target))
            {
                ChangeMonsterState(MonsterChaseState.Instance);
            }
            else
            {
                ChangeMonsterState(MonsterNormalState.Instance);
            }
        }
    }

    //몬스터 달리기 애니메이션을 재생합니다.
    public void PlayRunAnimation()
    {
        animator.SetBool(animatorIsRunParamName, true);
        animator.SetBool(animatorIsWalkParamName, false);
        animator.SetBool(animatorIsBattleParamName, false);
    }

    //몬스터의 상태를 변경합니다.
    public void ChangeMonsterState(IFSMState<MonsterController> _State)
    {
        if (fsm != null && _State != null)
            fsm.ChangeState(_State);
    }
}
