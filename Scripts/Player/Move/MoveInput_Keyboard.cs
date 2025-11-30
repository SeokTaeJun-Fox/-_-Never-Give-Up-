using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pc버전에서 입력된 키보드의 값으로인한 방향벡터를 불러옵니다.
[CreateAssetMenu(fileName = "KeyboardMove", menuName = "Scriptable Object/MoveInputType/KeyboardMove")]
public class MoveInput_Keyboard : MoveInput
{
    float x, z;
    public override Vector3 GetMoveDir()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0, z).normalized;
    }
}
