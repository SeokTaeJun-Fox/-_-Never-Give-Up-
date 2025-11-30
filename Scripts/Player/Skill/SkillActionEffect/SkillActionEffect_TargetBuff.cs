using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대상에게 버프를 주는 효과입니다.
[CreateAssetMenu(fileName = "SAE_TargetBuff_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_TargetBuff")]
public class SkillActionEffect_TargetBuff : SkillActionEffect
{
    [SerializeField] private Buff buff; //대상에게 줄 버프

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        _target.TakeBuff(buff, _user.Ability());
    }
}
