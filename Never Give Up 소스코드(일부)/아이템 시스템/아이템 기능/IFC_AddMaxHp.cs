using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최대 체력 추가 효과를 가진 아이템 기능 요소 클래스입니다.
//사용 시 추가 최대 체력을 증가시키고, 해제 시 감소시킵니다.
[CreateAssetMenu(fileName = "AddMaxHp_", menuName = "Scriptable Object/Item/ItemFunctionElement/AddMaxHp")]
public class IFC_AddMaxHp : ItemFunctionElement
{
    //최대 체력 증가 효과를 적용합니다.
    public override void Use(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddMaxHp(amount);
    }

    //최대 체력 증가 효과를 제거합니다.
    public override void UnUse(IItemUser _user)
    {
        if (_user != null)
            _user.IncreaseAddMaxHp(-amount);
    }
}
