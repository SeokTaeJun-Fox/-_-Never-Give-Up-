using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프 컨트롤러의 핵심 기능을 정의하는 인터페이스입니다.
//버프 적용, 제거, 일시정지, 재시작, 조회 기능을 제공합니다.
public interface IBuffController
{
    event Action<ActiveBuff> OnGetBuff; //버프 부여시 이벤트가 발생됩니다.
    IReadOnlyList<ActiveBuff> ActiveBuffs { get; }

    void SetOwner(IBuffUser _owner);

    void Stop();    //버프 일시정지
    void Play();    //버프 재시작
    void GetBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility, ICharacterSoundController _sfxSoundController);   //버프 부여
    void ClearBuff(Buff _buff); //해당 버프 제거
    void ClearBuff(List<Category> _buffCategoryList);   //카테고리 기반 버프 제거
    void ClearAllBuff();    //모든 버프 제거
}
