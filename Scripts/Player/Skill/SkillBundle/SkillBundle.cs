using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여러 스킬 레벨 그룹을 포함하는 스크립터블 오브젝트입니다.
/// 스킬 ID로 그룹을 조회하거나 전체 복제 기능을 제공합니다.
/// </summary>
[CreateAssetMenu(fileName = "SkillBundle", menuName = "Scriptable Object/PlayerSkill/SkillBundle/SkillBundle")]
public class SkillBundle : ScriptableObject
{
    [SerializeField] private List<SkillLevelGroup> skillLevelGroups;

    public List<SkillLevelGroup> SkillLevelGroups { get => skillLevelGroups; }

    /// <summary>
    /// 스킬 ID에 해당하는 그룹을 반환합니다.
    /// </summary>
    public SkillLevelGroup FindLevelGroup(string _skillId)
    { 
        return skillLevelGroups.Find(x => x.SkillId == _skillId);
    }

    /// <summary>
    /// 스킬 번들을 복제합니다.
    /// </summary>
    public SkillBundle Clone()
    { 
        SkillBundle clone = Instantiate(this);
        clone.skillLevelGroups = skillLevelGroups.ConvertAll(x => x.Clone());
        return clone;
    }
}
