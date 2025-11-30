using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : FSMSingleton<PlayerDamagedState>, IFSMState<PlayerController>
{
    public void Enter(PlayerController e)
    {
        e.PlayerRandomIdle.CanRandomIdle = false;
        e.PlayerMovement.SetCanMoveAndJump(false);
        e.SkillController.SetActiveSkillController(false);
    }

    public void Execute(PlayerController e)
    {
        if (e.RemainDamageActionTime < 0)
        {
            e.ChangePlayerState(PlayerNormalState.Instance);
        }

        e.RemainDamageActionTime -= Time.deltaTime;
    }

    public void Exit(PlayerController e)
    {
        e.PlayDamageAnimation(false);
    }
}
