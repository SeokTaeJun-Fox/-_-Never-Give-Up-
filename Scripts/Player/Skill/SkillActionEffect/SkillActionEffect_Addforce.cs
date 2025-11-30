using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자 기준 방향으로 대상에게 물리적 힘을 가하는 스킬 효과입니다.
//넉백, 밀기 등의 효과에 사용됩니다.
[CreateAssetMenu(fileName = "skillActionEffect_Addforce_", menuName = "Scriptable Object/PlayerSkill/SkillActionEffect/SkillActionEffect_Addforce")]
public class SkillActionEffect_Addforce : SkillActionEffect
{
    [SerializeField] private Vector3 forceDir;  //사용자 기준 힘의 방향
    [SerializeField] private float power;   //힘의 세기

    //대상에게 사용자 기준 방향으로 힘을 가합니다.
    public override void ActionEffect(ISkillUser _user, Damageable _target)
    {
        Vector3 force = _user.Owner().TransformDirection(forceDir) * power;
        Debug.Log(force);
        _target.Addforce(force);
    }
}
