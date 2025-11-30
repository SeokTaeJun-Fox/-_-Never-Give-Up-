using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : FSMSingleton<PlayerDeadState>, IFSMState<PlayerController>
{
    public void Enter(PlayerController e)
    {
        e.PlayerRandomIdle.CanRandomIdle = false;
        e.SkillController.SetActiveSkillController(false);
        e.PlayerMovement.SetCanMoveAndJump(false);
        e.PlayerMovement.enabled = false;
    }

    public void Execute(PlayerController e)
    {

    }

    public void Exit(PlayerController e)
    {

    }
}
