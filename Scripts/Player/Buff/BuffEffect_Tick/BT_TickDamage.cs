using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//지속 데미지 효과 (버프사용자의 능력치기준 n%의 지속데미지효과)
[CreateAssetMenu(fileName = "BT_", menuName = "Scriptable Object/Buff/BuffTick/BT_TickDamage")]
public class BT_TickDamage : BuffEffect_Tick
{
    [SerializeField] private PlayerStat standard;   //기준
    [SerializeField] private float rate;    //0~1

    public override void Tick(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        int damage = (int)((int)_providerAbility[standard] * rate);
        _user.Damage(damage, false, true);  //방어력 무시 데미지를 입힘니다.
    }
}
