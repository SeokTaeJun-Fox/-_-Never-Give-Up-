using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어를 위한 스킬액션입니다.
//예: 플레이어에게 버프걸기, 공격시 몬스터방향으로 자동회전등
[CreateAssetMenu(fileName = "skillAction_ForOwner_", menuName = "Scriptable Object/PlayerSkill/SkillAction/SkillAction_ForOwner")]
public class SkillAction_ForOwner : SkillAction
{
    //사용자에게만 효과를 적용합니다. 대상은 null로 전달됩니다.
    public override void Action(ISkillUser _user)
    {
        foreach (SkillActionEffect skillActionEffect in skillActionEffects)
            skillActionEffect.ActionEffect(_user, null);
    }
}
