using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 시전시 시전 파티클 생성하는 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "SAE_PlaySkillParticleOneshot_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_PlaySkillParticleOneshot")]
public class SkillActionEffect_PlaySkillParticleOneshot : SkillActionEffect
{
    [SerializeField] private string particlePoolName;
    [SerializeField] private Vector3 localPos;
    [SerializeField] private float masterScale;
    [SerializeField] private bool isCancelable;
    [SerializeField] private bool isOwner;  //대상은 자신인가? 아님 타겟인지 확인합니다.

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        if (isOwner)
            _user.PlayParticleOneShot(particlePoolName, localPos, isCancelable, masterScale);
        else
            _target.PlayParticleOneShot(particlePoolName, localPos, isCancelable, masterScale);
    }
}
