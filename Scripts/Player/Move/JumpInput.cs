using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpInput : ScriptableObject
{
    //현재 키보드 혹은 버튼이 눌렀는지 확인한다
    public abstract bool IsJumpButtonPressed();
}
