using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동 기능이 있는 스크립터블 오브젝트입니다.
public abstract class ActionObject : ScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명
    public abstract void Action();
}
