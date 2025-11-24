using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 관리 기능을 정의하는 인터페이스입니다.
/// 스킬 획득, 레벨업, 포인트 관리 및 이벤트를 제공합니다.
/// </summary>
public interface ISkillManager
{
    Skill GetSkill(string _skillId, int level);    //특정 레벨의 스킬반환
    void PossessSkill(string _skillId, int level);  //스킬 획득
    void SkillLevelup(string _skillId, bool isUseSkillPoint = true); //스킬 레벨업
    int GetMaxLevel(string _skillId);  //최대 레벨 조회

    int GetSkillPoint { get; }
    int SetSkillPoint { set; }

    //ID로 해당 플레이어가 보유중인 스킬을 조회합니다.
    IReadOnlyDictionary<string, Skill> GetCurrentSkill();

    event Action<Skill> OnSkillLevelUp; //스킬 레벨업 이벤트
    event Action<int> OnSkillPointUse;  //스킬 포인트 사용 이벤트

    void Initial();
}
