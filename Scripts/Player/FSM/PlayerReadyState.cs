using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyState : FSMSingleton<PlayerReadyState>, IFSMState<PlayerController>
{
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
