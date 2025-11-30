using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FSM<PlayerController>
{
    private void Update()
    {
        FSMUpdate();
    }
}
