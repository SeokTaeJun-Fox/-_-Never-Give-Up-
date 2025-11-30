using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 스킬 공격상태 클래스입니다.
public class PlayerSkillAttackState : FSMSingleton<PlayerSkillAttackState>, IFSMState<PlayerController>
{
    float remainAttackTime; //공격상태가 끝날때까지 남은 시간

    public void Enter(PlayerController e)
    {
        e.PlayerRandomIdle.CanRandomIdle = false;
        e.PlayerMovement.SetCanMoveAndJump(false);
    }

    public void Execute(PlayerController e)
    {

    }

    public void Exit(PlayerController e)
    {

    }
}
