using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 몬스터나 오브젝트를 처치할시 경험치를 획득하는 클래스입니다.
public class ExpGet : MonsterInfoUser
{
    [SerializeField] private int exp;

    public override void Setting(MonsterInfo _monsterInfo, IReadOnlyDictionary<PlayerStat, object> _monsterAbilities)
    {
        exp = _monsterInfo.Exp;
    }

    public void GetExp()
    {
        FindObjectOfType<PlayerAbilityManager>().AddExp(exp); //플레이어 능력치 매니저에게 전달
        Debug.Log($"exp : {exp} get");
    }
}
