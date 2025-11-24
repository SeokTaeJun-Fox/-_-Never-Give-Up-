using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 상태 처리는 FSM패턴으로 구현했습니다.
//몬스터의 일반 상태입니다.
//순찰을 수행하거나 타겟을 탐지하여 추격 상태로 전환합니다.
//Enter: 상태 시작시 호출
//Execute: 상태 진행중 매 프레임마다 호출
//Exit: 상태 끝날시 호출
public class MonsterNormalState : FSMSingleton<MonsterNormalState>, IFSMState<MonsterController>
{
    //상태 진입 시 순찰 초기화 및 준비 상태로 전환합니다.
    public void Enter(MonsterController e)
    {
        e.PatrolFunction.PatrolReset();
        e.PatrolFunction.PatrolReadyOrPatrol();
    }

    //순찰을 수행하거나 타겟을 탐지하여 상태를 전환합니다.
    //(매 프레임마다 호출)
    public void Execute(MonsterController e)
    {
        
        if (!e.PatrolFunction.IsPatrol)
        {
            if (e.PatrolFunction.RemainPatrolReadyTime < 0)
                e.PatrolFunction.Patrol();
            else
                e.PatrolFunction.DecreaseRemainPatrolReady(Time.deltaTime);
        }
        else
        {
            if (e.PatrolFunction.CheckArrivePatrolDestination())
                e.PatrolFunction.PatrolReadyOrPatrol();
        }

        Transform target = null;

        if (e.ChaseAttackController.CheckFindTarget(out target))
        {
            e.Agent.SetDestination(target.position);
            e.ChangeMonsterState(MonsterChaseState.Instance);   //추격 상태로 전환
        }
    }

    public void Exit(MonsterController e)
    {

    }
}
