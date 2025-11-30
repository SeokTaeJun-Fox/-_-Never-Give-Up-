using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//발사체 생성 스킬행동클래스입니다.
[CreateAssetMenu(fileName = "SkillAction_CreateProjectile_", menuName = "Scriptable Object/PlayerSkill/SkillAction/SkillAction_CreateProjectile")]
public class SkillAction_CreateProjectile : SkillAction
{
    [SerializeField] protected ObjectPoolEvent objectPoolEvent;
    [SerializeField] protected string projectileObjectName;   //생성할 발사체오브젝트명(오브젝트 풀에 사용됩니다.)

    [SerializeField] private Vector3 startOffset;   //시작 위치 오프셋(스킬 사용자 기준)

    [SerializeField] protected LayerMask targetLayer; //발사체에게 입힐 타겟 레이어
    [SerializeField] LayerMask collideLayer;    //발사체는 특정 레이어에 부딫히면 폭발합니다. 이것을 알려주는 변수입니다.
    [SerializeField] private float powerMultiple;  //공격력 배수
    [SerializeField] private float spd;      //속도
    [SerializeField] private float liveTime; //생존시간

    public override void Action(ISkillUser _user)
    {
        Vector3 startPos = _user.Owner().position + startOffset;
        GameObject go = objectPoolEvent.RaiseGet(projectileObjectName, startPos, _user.Owner().rotation);
    
        DO_Projectile projectile = go.GetComponent<DO_Projectile>();
        if (projectile != null)
        {
            int power = (int)((int)_user.Ability()[PlayerStat.TOTAL_ATK] * powerMultiple);
            Vector3 dir = _user.Owner().forward.normalized;
            projectile.Setting(power, dir, spd, liveTime, collideLayer, targetLayer);
        }
        else
        {
            Debug.LogWarning($"skillAction : {name}, 발사체 {projectileObjectName}오브젝트는 DO_Projectile이 부착되어있지 않습니다.");
        }
    }
}
