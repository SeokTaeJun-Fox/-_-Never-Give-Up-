using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 적 방향으로 자동 회전됩니다.
[CreateAssetMenu(fileName = "skillEffect_AutoRotateToEnemy", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_AutoRotateToEnemy")]
public class SkillActionEffect_AutoRotateToTarget : SkillActionEffect
{
    [SerializeField] private float addDistance;   //플레이어 중앙기준 공격 추가 범위 (공격범위 = 무기 공격범위 + addDistance)
    [SerializeField] private LayerMask layerMask;   //범위안에 어떤 레이어형 오브젝트에게 효과를 주는지 설정합니다.

    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        Transform owner = _user.Owner();

        float attackDistance = (float)_user.Ability()[PlayerStat.ATTACKRANGE] + addDistance;
        Collider[] colliders = Physics.OverlapSphere(owner.position, attackDistance, layerMask);

        if (colliders.Length != 0)
        {
            owner.transform.rotation = Quaternion.LookRotation(colliders[0].transform.position - owner.position, Vector3.up);
            owner.transform.eulerAngles = new Vector3(0, owner.transform.eulerAngles.y, 0);
        }
    }
}
