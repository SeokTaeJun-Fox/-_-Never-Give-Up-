using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//조건 스크립터블 오브젝트
public abstract class ConditionObject : ScriptableObject
{
    [SerializeField] private string description;

    public abstract bool IsPass();
}
