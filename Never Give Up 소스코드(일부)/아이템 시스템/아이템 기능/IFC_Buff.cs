using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프를 적용하는 아이템 기능 요소 클래스입니다.
//사용 시 대상에게 해당 Buff를 적용합니다.
[CreateAssetMenu(fileName = "IFC_Buff_", menuName = "Scriptable Object/Item/ItemFunctionElement/IFC_Buff")]
public class IFC_Buff : ItemFunctionElement
{
    [SerializeField] private Buff buff;

    public override void UnUse(IItemUser _user)
    {

    }

    //버프를 적용합니다. 아이템 사용자의 능력치도 함께 전달됩니다.
    public override void Use(IItemUser _user)
    {
        _user.TakeBuff(buff, _user.PlayerAbilities);
    }
}
