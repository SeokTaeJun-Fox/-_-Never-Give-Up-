using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pc버전에서 점프키를 입력했는지 확인하는 클래스
[CreateAssetMenu(fileName = "KeyboardJump", menuName = "Scriptable Object/MoveInputType/KeyboardJump")]
public class JumpInput_Keyboard : JumpInput
{
    public override bool IsJumpButtonPressed()
    {
        return Input.GetButtonDown("Jump");
    }
}
