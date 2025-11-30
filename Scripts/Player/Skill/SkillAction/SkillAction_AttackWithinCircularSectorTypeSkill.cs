using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//부채꼴 범위 내의 대상에게 효과를 적용하는 스킬 액션입니다.
//공격 범위는 무기 사거리 + 추가 거리이며, 지정된 레이어의 대상만 처리됩니다.
[CreateAssetMenu(fileName = "skillAction_CircularSectorAttack_", menuName = "Scriptable Object/PlayerSkill/SkillAction/AttackWithinCircularSectorTypeSkill")]
public class SkillAction_AttackWithinCircularSectorTypeSkill : SkillAction
{
    [SerializeField] private float addDistance;   //플레이어 중앙기준 공격 추가 범위 (공격범위 = 무기 공격범위 + addDistance)
    [SerializeField] private float degree;   //플레이어 중앙기준 범위(각도)
    [SerializeField] private LayerMask layerMask;   //범위안에 어떤 레이어형 오브젝트에게 효과를 주는지 설정합니다.

    public override void Action(ISkillUser _user)
    {
        Transform owner = _user.Owner();

        float attackDistance = (float)_user.Ability()[PlayerStat.ATTACKRANGE] + addDistance;
        Collider[] colliders = Physics.OverlapSphere(owner.position, attackDistance, layerMask);
        foreach (Collider col in colliders)
        {
            Vector3 distanceVector = col.transform.position - owner.position;
            distanceVector = Vector3.Normalize(distanceVector);

            float myAngle = Utility.GetDegree(owner.forward, distanceVector);

            if (myAngle < degree)
            {
                Damageable target = col.GetComponent<Damageable>();
                if (target != null)
                {
                    foreach (SkillActionEffect effect in skillActionEffects)
                    {
                        effect.ActionEffect(_user, target);
                    }
                }
            }
        }
    }
}
