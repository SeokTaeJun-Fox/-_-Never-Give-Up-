using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveInput : ScriptableObject
{
    //현재 키보드 혹은 조이스틱으로 입력된 방향 벡터를 불러옵니다.
    public abstract Vector3 GetMoveDir();
}
