using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격력 증가 기능을 가진 아이템 기능 요소 클래스입니다.
/// 사용 시 공격력 추가, 해제 시 감소 처리합니다.
/// </summary>
[CreateAssetMenu(fileName = "AddAtk_", menuName = "Scriptable Object/Item/ItemFunctionElement/AddAtk")]
public class IFC_AddAtk : ItemFunctionElement
{
    //공격력 증가 효과를 적용합니다.
    public override void Use(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddAtk(amount);
    }

    //공격력 증가 효과를 제거합니다.
    public override void UnUse(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddAtk(-amount);
    }
}
