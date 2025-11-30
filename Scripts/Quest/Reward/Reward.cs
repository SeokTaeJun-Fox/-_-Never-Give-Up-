using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField] private Sprite icon;   //아이콘 스프라이트
    [SerializeField] private string description;    //설명
    [SerializeField] private int quantity;  //수량

    public virtual Sprite Icon => icon;
    public virtual string Description => description;
    public int Quantity => quantity;

    public abstract void Give(Quest quest);
}
