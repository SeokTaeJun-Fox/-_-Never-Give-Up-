using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터의 공격 상태입니다.
//공격 시작 시 스킬 컨트롤러를 활성화하고, 공격을 수행합니다.
//Enter: 상태 시작시 호출
//Execute: 상태 진행중 매 프레임마다 호출
//Exit: 상태 끝날시 호출
public class MonsterAttackState : FSMSingleton<MonsterAttackState>, IFSMState<MonsterController>
{
    //상태 진입 시 공격을 시작하고 이동을 멈춥니다.
    public void Enter(MonsterController e)
    {
        e.ChaseAttackController.AttackStart();
        e.Agent.isStopped = true;
    }

    public void Execute(MonsterController e)
    {

    }
    
    //상태 종료 시 공격을 중단합니다.
    public void Exit(MonsterController e)
    {
        e.ChaseAttackController.AttackStop();
    }
}
