using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : FSMSingleton<PlayerNormalState>, IFSMState<PlayerController>
{
    public void Enter(PlayerController e)
    {
        e.PlayerRandomIdle.CanRandomIdle = true;
        e.PlayerMovement.SetCanMoveAndJump(e.CanControl);
        e.SkillController.SetActiveSkillController(e.CanControl);
    }

    public void Execute(PlayerController e)
    {
        e.PlayerMovement.SetCanMoveAndJump(e.CanControl);
        e.SkillController.SetActiveSkillController(e.CanControl);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            e.SkillController.NormalAttack();
            //e.ChangePlayerState(PlayerAttackState.Instance);
        }

        if (Input.GetKeyDown(KeyCode.Z) && e.CanControl)
        {
            e.PlayerItemPickUp.PickUpItem();
        }
    }

    public void Exit(PlayerController e)
    {

    }
}
