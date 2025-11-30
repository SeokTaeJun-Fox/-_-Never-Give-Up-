using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대상에게 데미지를 입히는 스킬 효과입니다.
//공격력 계수를 기반으로 피해량을 계산합니다.
[CreateAssetMenu(fileName = "skillEffect_Hit_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_Hit")]
public class SkillActionEffect_Hit : SkillActionEffect
{
    [SerializeField] float damageAmount;    //피해량 (기본 1)

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        int damage = (int)((int)_user.Ability()[PlayerStat.TOTAL_ATK] * damageAmount);
        _target.Damage(damage);
    }
}
