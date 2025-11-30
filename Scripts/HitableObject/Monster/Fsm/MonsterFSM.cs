using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : FSM<MonsterController>
{
    private void Update()
    {
        FSMUpdate();
    }
}
