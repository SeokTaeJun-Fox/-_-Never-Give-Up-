using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템의 기능 요소를 정의하는 추상 클래스입니다.
//능력치 변화나 버프 적용 등 실제 효과를 구현하며, 중첩 여부와 UI출력용 정보를 포함합니다.
//개방 폐쇄 원칙을 적용하여 새로운 아이템 효과는 상속받아 확장 할 수 있고,
//기존 코드 수정 없이 기능 추가를 할 수 있도록 설계했습니다.
public abstract class ItemFunctionElement : ScriptableObject
{
    [SerializeField] private bool isOverlap;    //같은 기능끼리 중첩가능한지 여부
    [SerializeField] private string category;   //ui 출력용 카테고리 이름 (예: 공격력)
    [SerializeField] protected int amount;      //UI 출력용 수치 (예: 15)   

    //category,amount변수는 장비 상세정보 창에 출력하는데 사용됩니다.
    //예: 공격력: 15

    public bool IsOverlap => isOverlap;
    public string Category { get => category; }
    public int Amount { get => amount; }

    public abstract void Use(IItemUser _user);
    public abstract void UnUse(IItemUser _user);
}
