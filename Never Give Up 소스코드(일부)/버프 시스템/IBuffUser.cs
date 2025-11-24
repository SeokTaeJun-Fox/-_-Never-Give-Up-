using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프 효과를 적용받는 대상이 구현해야 하는 인터페이스입니다.
//능력치 변화, 파티클 연출, 피해 처리 등을 정의합니다.
public interface IBuffUser
{
    void Damage(int _damage, bool _isDamageAction = true, bool _isIgnoreDef = false);   //피해 처리
    void IncreaseHp(int _amount);   //체력 증가
    void IncreaseAddAtk(int _amount);   //공격력 증가 (기초 공격력 증가가 아닌 추가 공격력 증가)
    void IncreaseAddDef(int _amount);   //방어력 증가 (기초 방어력 증가가 아닌 추가 방어력 증가)
    IReadOnlyDictionary<PlayerStat, object> Ability();  //현재 능력치 정보

    //파티클 연출
    void PlayParticleOn(string _particlePoolName, string _code, float _masterScale);
    void PlayParticleOff(string _code);
}
