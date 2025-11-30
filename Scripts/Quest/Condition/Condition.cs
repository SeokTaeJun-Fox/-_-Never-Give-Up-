using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//조컨 클래스 (ex : 퀘스트를 받을수있는 최소 플레이어 레벨)
public abstract class Condition : ScriptableObject
{
    [SerializeField] private string description;

    public abstract bool IsPass(Quest quest);
}
