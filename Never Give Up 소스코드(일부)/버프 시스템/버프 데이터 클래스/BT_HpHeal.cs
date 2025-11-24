using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//지속적으로 체력을 회복하는 버프 효과 클래스입니다.
//Tick 주기마다 체력을 회복시킵니다.
[CreateAssetMenu(fileName = "BT_", menuName = "Scriptable Object/Buff/BuffTick/BT_HpHeal")]
public class BT_HpHeal : BuffEffect_Tick
{
    [SerializeField] private int amount;

    public override void Tick(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        _user.IncreaseHp(amount);
    }
}
