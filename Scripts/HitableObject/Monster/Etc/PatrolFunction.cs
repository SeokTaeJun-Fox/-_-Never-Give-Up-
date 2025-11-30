using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolFunction : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private RuntimeAnimatorController ao;

    //해당 애니메이터 파라미터명
    [SerializeField] private string animatorIsWalkParamName;
    [SerializeField] private string animatorIsRunParamName;
    [SerializeField] private string animatorIsBattleParamName;
    [SerializeField] private string animatorAttackParamName;
    [SerializeField] private string animatorDamageName;
    [SerializeField] private string animatorDamageEndName;
    [SerializeField] private string animatorDieParamName;

    //순찰
    [SerializeField] Transform[] patrolPos; //순찰 위치

    [SerializeField] float patrolReadyMaxTime;  //패트롤 대기시간 최대치
                                                //패트롤 대기시간은 0~patrolReadyMaxTime초입니다.
    [SerializeField] float patrolspeed; //순찰 속도

    private float remainPatrolReadyTime;    //남은 패트롤 대기시간
    private Transform curPatrolPos;    //현재 패트롤 인덱스
    private bool isPatrol;      //현재 순찰중인지 확인한다.
    private List<Transform> patrolPossiblePos;  //현재 순찰 가능한 장소들

    public bool IsPatrol => isPatrol;
    public float RemainPatrolReadyTime => remainPatrolReadyTime;
    public float patrolSpeed => patrolspeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

        patrolPossiblePos = new List<Transform>();

        isPatrol = false;
        remainPatrolReadyTime = 0;
        curPatrolPos = null;
    }

    public void Setting(Transform[] _patrolPos)
    { 
        patrolPos = _patrolPos;
    }

    //패트롤 혹은 패트롤대기한다
    public void PatrolReadyOrPatrol()
    {
        //isPatrolReady의값이 0이면 패트롤, 1이면 패트롤대기입니다.
        int isPatrolReady = Random.Range(0, 2);

        if (isPatrolReady == 0)
        {
            Patrol();
        }
        else
        {
            PatrolReady();
        }
    }

    //순찰한다.
    public void Patrol()
    {
        agent.isStopped = false;
        agent.speed = patrolSpeed;

        //patrolPossiblePos에 순찰 가능 위치를 저장합니다.
        //현재 1번순찰 완료했으면 1번을 다시 순찰할수 없기때문에
        //순찰 가능 위치에서 제외됩니다.
        patrolPossiblePos.Clear();
        for (int num = 0; num < patrolPos.Length; num++)
        {
            if (curPatrolPos == null || !curPatrolPos.name.Equals(patrolPos[num].name))
                patrolPossiblePos.Add(patrolPos[num]);
        }

        //다음순찰위치를 랜덤으로 정합니다.
        int patrolIndex = Random.Range(0, patrolPossiblePos.Count);

        //Debug.Log($"순찰시작 : {patrolIndex}, {patrolPossiblePos[patrolIndex]}");

        //순찰합니다
        agent.SetDestination(patrolPossiblePos[patrolIndex].position);

        //애니메이션을 실행합니다.
        animator.SetBool(animatorIsBattleParamName, false);
        animator.SetBool(animatorIsWalkParamName, true);
        animator.SetBool(animatorIsRunParamName, false);

        curPatrolPos = patrolPossiblePos[patrolIndex];
        isPatrol = true;
    }

    //순찰대기합니다.
    public void PatrolReady()
    {
        agent.isStopped = true;
        remainPatrolReadyTime = Random.Range(0, patrolReadyMaxTime);

        //Debug.Log($"순찰 대기 : {remainPatrolReadyTime}");

        //애니메이션을 실행합니다.
        animator.SetBool(animatorIsBattleParamName, false);
        animator.SetBool(animatorIsWalkParamName, false);
        animator.SetBool(animatorIsRunParamName, false);

        isPatrol = false;
    }

    //남은 순찰대기시간 감소합니다.
    public void DecreaseRemainPatrolReady(float _amount)
    {
        remainPatrolReadyTime -= _amount;
    }

    //현재 순찰위치에 도착했는지 확인합니다.
    public bool CheckArrivePatrolDestination()
    {
        if (curPatrolPos == null)
        {
            Debug.LogWarning($"몬스터 : {name}가 순찰하려는 목적지가 없습니다.");
            return false;
        }

        //if (Vector3.SqrMagnitude(transform.position - curPatrolPos.position) < 1)
        if (agent.remainingDistance <= agent.stoppingDistance)
            return true;
        else
            return false;
    }

    //순찰을 리셋합니다.
    public void PatrolReset()
    {
        curPatrolPos = null;
        agent.stoppingDistance = 0;

        if (isPatrol)
        {
            agent.ResetPath();
            isPatrol = false;
        }
    }
}
