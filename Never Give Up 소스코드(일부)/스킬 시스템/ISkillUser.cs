using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬을 사용할 수 있는 대상이 구현해야 하는 인터페이스 입니다.
//위치 정보, 능력치, 파티클, 버프 적용 기능을 제공합니다.
public interface ISkillUser
{
    Transform Owner();  //스킬 사용자의 Transform을 반환합니다.
    IReadOnlyDictionary<PlayerStat, object> Ability();  //스킬 사용자의 능력치를 반환합니다.

    //스킬 사용자에서 파티클을 1회 재생합니다.
    void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale);
    
    //스킬사용자에게 버프를 적용합니다.
    void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility);
}
