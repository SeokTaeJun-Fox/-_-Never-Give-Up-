using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 동일한 스킬의 레벨별 정보를 담고 있는 스크립터블 오브젝트입니다.
/// </summary>
[CreateAssetMenu(fileName = "SkillLevelGroup", menuName = "Scriptable Object/PlayerSkill/SkillBundle/SkillLevelGroup")]
public class SkillLevelGroup : ScriptableObject
{
    [SerializeField] private string skillId;    //스킬 고유 ID
    [SerializeField] private List<Skill> skills;    //레벨별 스킬 리스트

    public string SkillId { get => skillId; }
    public List<Skill> Skills { get => skills; }

    /// <summary>
    /// 스킬 그룹을 복제합니다
    /// </summary>
    public SkillLevelGroup Clone()
    {
        SkillLevelGroup clone = Instantiate(this);
        clone.skills = skills.ConvertAll((x) => Instantiate(x));
        clone.skillId = skillId;
        return clone;
    }
}
