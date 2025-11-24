using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 효과 클래스입니다.
//예: 플레이어기준 부채꼴 범위내에 적들에게 일정량의 데미지, 일정범위에 피해 오브젝트 소환등) 
public abstract class SkillAction : ScriptableObject
{
    //스킬 시전 중 실행되는 효과입니다.
    //하위 클래스에서 구체적인 로직을 구현합니다.
    public abstract void Action(ISkillUser _user);

    //스킬 시전 시작시 실행되는 효과입니다.
    //예: 시전 이펙트, 버프 적용 등
    public void OnStart(ISkillUser _user)
    {
        foreach(var skillActionEffect in StartskillActionEffects)
            skillActionEffect.ActionEffect(_user, null);
    }

    //시전 시작 시 효과
    [SerializeField] protected List<SkillActionEffect> StartskillActionEffects;

    //시전 중 특정 시점의 효과
    [SerializeField] protected List<SkillActionEffect> skillActionEffects;
}
