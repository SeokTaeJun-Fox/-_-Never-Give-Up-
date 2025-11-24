using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자 자신에게 버프를 적용하는 스킬 효과입니다.
[CreateAssetMenu(fileName = "SAE_SelfBuff_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_SelfBuff")]
public class SkillActionEffect_SelfBuff : SkillActionEffect
{
    [SerializeField] private Buff buff; //자신에게 적용할 버프

    //사용자에게 지정된 버프를 적용합니다.
    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        _user.TakeBuff(buff, _user.Ability());
    }
}
