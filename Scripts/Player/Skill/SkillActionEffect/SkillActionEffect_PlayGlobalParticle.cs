using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//글로벌 위치에 파티클 생성 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "SAE_PlayGlobalParticle_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_PlayGlobalParticle")]
public class SkillActionEffect_PlayGlobalParticle : SkillActionEffect
{
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private string particlePoolName;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isPosOwner;  //스킬 사용자위치에 파티클을 배치하려면 true
                                               //상대 사용자위치에 파티클을 배치하려면 false

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        if (isPosOwner && _user != null)
        {
            poolEvent.RaiseGet(particlePoolName, _user.Owner().position + offset, Quaternion.identity);
        }
        else if (!isPosOwner && _target != null)
        {
            poolEvent.RaiseGet(particlePoolName, _target.transform.position + offset, Quaternion.identity);
        }
    }
}
