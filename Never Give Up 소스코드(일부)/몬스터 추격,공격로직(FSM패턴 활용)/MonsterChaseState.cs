using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터의 추격 상태입니다.
//타겟을 향해 이동하며, 공격 범위에 도달하면 공격 상태로 전환합니다.
public class MonsterChaseState : FSMSingleton<MonsterChaseState>, IFSMState<MonsterController>
{
    //상태 진입 시 추격 설정을 초기화하고 달리기 애니메이션을 재생합니다.
    public void Enter(MonsterController e)
    {
        e.ChaseAttackController.ChaseReset();
        e.ChaseAttackController.PlayRunAnimation();

        e.Agent.isStopped = false;
    }

    //상태 중 매 프레임마다 타겟을 추적하며,
    //조건에 따라 상태를 전환합니다.
    public void Execute(MonsterController e)
    {
        Transform target;
        //플레이어를 발견하면 추격 목적지는 플레이어위치에 설정합니다.
        if (e.ChaseAttackController.CheckFindTarget(out target))
        {
            e.Agent.SetDestination(target.position);
        }

        //플레이어를 발견하지 못하고, 추격 목적지 근처(stoppingDistance)에 도달하면
        //추격을 포기하고 노말상태가 됩니다.
        if (!e.ChaseAttackController.CheckFindTarget(out target) && 
            e.Agent.remainingDistance <= e.Agent.stoppingDistance)
        {
            e.ChangeMonsterState(MonsterNormalState.Instance);
        }

        //플레이어를 발견하고, 공격 거리 내에있으면
        //몬스터가 플레이어를 공격할수있다고 판단해
        //공격상태가 됩니다.
        else if (e.ChaseAttackController.CheckFindTarget(out target) &&
            Vector3.Distance(e.transform.position, target.position) <= e.MaxAttackDistance)
        {
            e.ChangeMonsterState(MonsterAttackState.Instance);
        }
    }

    public void Exit(MonsterController e)
    {

    }
}
