using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 동일한 스킬의 레벨별 정보를 담고 있는 스크립터블 오브젝트입니다.
// 각 스킬은 Skill 클래스(스크립터블오브젝트) 인스턴스로 구성되며, 스킬 ID를 기준으로 그룹화됩니다.
[CreateAssetMenu(fileName = "SkillLevelGroup", menuName = "Scriptable Object/PlayerSkill/SkillBundle/SkillLevelGroup")]
public class SkillLevelGroup : ScriptableObject
{
    [SerializeField] private string skillId;    //스킬 고유 ID
    [SerializeField] private List<Skill> skills;    //레벨별 스킬 리스트

    public string SkillId { get => skillId; }
    public List<Skill> Skills { get => skills; }

    // 스킬 그룹을 복제합니다.
    // 쿨타임 등 실시간 값이 변경되어 원본 데이터값이 변경되므로, Skill 오브젝트를 복제하여 독립적으로 관리합니다.
    public SkillLevelGroup Clone()
    {
        SkillLevelGroup clone = Instantiate(this);
        clone.skills = skills.ConvertAll((x) => Instantiate(x));
        clone.skillId = skillId;
        return clone;
    }
}
