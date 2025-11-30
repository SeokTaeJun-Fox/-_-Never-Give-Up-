using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 효과를 적용받는 대상이 구현해야 하는 인터페이스입니다.
//능력치 변화, 버프 적용/제거 등의 기능을 정의합니다.
public interface IItemUser
{
    //현재 능력치 정보
    IReadOnlyDictionary<PlayerStat, object> PlayerAbilities { get; }

    //체력/마나 회복
    void IncreaseHp(int _amount);
    void IncreaseMp(int _amount);

    //최대 체력 / 최대 마나 증가 (기초 체력이,마나가 아닌 추가 최대 체력,마나 증가)
    void IncreaseAddMaxHp(int _amount);
    void IncreaseAddMaxMp(int _amount);

    //공격력 / 방어력 증가 (기초 공격력,방어력이 아닌 추가 최대 공격력, 방어력 증가)
    void IncreaseAddAtk(int _amount);
    void IncreaseAddDef(int _amount);

    void ChangeAttackRange(int _amount);    //공격 범위 변경
    void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility);    //버프 부여
    void ClearBuff(List<Category> _categories); //카테고리에 해당하는 버프 제거
}
