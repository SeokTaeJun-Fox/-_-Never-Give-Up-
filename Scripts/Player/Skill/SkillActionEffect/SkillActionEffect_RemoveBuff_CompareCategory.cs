using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대상에게 이로운 버프 제거효과입니다.
[CreateAssetMenu(fileName = "SAE_RemoveBuff(Category)", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_RemoveBuff_CompareCategory")]
public class SkillActionEffect_RemoveBuff_CompareCategory : SkillActionEffect
{
    [SerializeField] private List<Category> removeBuffCategoryList;

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        _target.ClearBuff(removeBuffCategoryList);
    }
}
