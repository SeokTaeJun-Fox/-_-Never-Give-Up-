using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격력 증가 버프 효과 클래스입니다.
//버프 시작 시 공격력을 증가시키고, 종료 시 감소시킵니다.
//BSE : BuffeffectStarEnd의 약자입니다.
[CreateAssetMenu(fileName = "BSE_", menuName = "Scriptable Object/Buff/BuffEffect_StartEnd/BSE_ATKIncrease")]
public class BSE_ATKIncrease : BuffEffect_StartEnd
{
    [SerializeField] private int amount;

    public override void TakeEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        _user.IncreaseAddAtk(amount);
    }

    public override void EndEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        _user.IncreaseAddAtk(-amount);
    }
}
