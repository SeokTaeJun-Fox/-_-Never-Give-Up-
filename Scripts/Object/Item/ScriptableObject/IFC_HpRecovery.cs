using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력 회복 효과를 가진 아이템 기능 요소 클래스입니다.
/// 사용 시 현재 체력을 회복시키며, 해제 효과는 없습니다.
/// </summary>
[CreateAssetMenu(fileName = "HpRecovery_", menuName = "Scriptable Object/Item/ItemFunctionElement/HpRecovery")]
public class IFC_HpRecovery : ItemFunctionElement
{
    //체력 회복 효과를 적용합니다.
    public override void Use(IItemUser _user)
    {
        if(_user != null)
            _user.IncreaseHp(amount);
    }

    public override void UnUse(IItemUser _user)
    {
        //회복은 일회성 효과이므로 비워둡니다.
    }
}
