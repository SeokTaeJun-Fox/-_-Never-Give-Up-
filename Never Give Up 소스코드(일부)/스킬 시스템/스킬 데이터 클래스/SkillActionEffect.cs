using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 액션을 구성하고있는 단일 효과 클래스입니다
//예: 데미지입히기, 버프 추가, 넉벡등 
public abstract class SkillActionEffect : ScriptableObject
{
    //스킬 효과를 실행합니다.
    //_user: 사용자, target: 상대
    public abstract void ActionEffect(ISkillUser _user, Damageable _target);
}
