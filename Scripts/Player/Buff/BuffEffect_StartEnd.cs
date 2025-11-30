using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프의 시작과 종료 시 실행되는 효과를 정의하는 추상 클래스입니다.
//예 : 공격력 증가, 방어력 감소 등
public abstract class BuffEffect_StartEnd : ScriptableObject
{
    public abstract void TakeEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility);
    public abstract void EndEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility);
}
