using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프의 지속 시간 동안 주기적으로 실행되는 효과를 정의하는 추상 클래스입니다.
//예: 초당 회복, 초당 피해 등
public abstract class BuffEffect_Tick : ScriptableObject
{
    public abstract void Tick(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility);
}
