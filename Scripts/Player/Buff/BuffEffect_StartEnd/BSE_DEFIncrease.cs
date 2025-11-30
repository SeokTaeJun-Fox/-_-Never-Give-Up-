using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//방어력 증가효과
[CreateAssetMenu(fileName = "BSE_", menuName = "Scriptable Object/Buff/BuffEffect_StartEnd/BSE_DEFIncrease")]
public class BSE_DEFIncrease : BuffEffect_StartEnd
{
    [SerializeField] private int amount;

    public override void TakeEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        _user.IncreaseAddDef(amount);
    }

    public override void EndEffect(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        _user.IncreaseAddDef(-amount);
    }
}
