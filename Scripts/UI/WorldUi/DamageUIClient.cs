using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIClient : OnDamagedEventUser
{
    [SerializeField] private Transform uiPos;
    [SerializeField] private DamageUIGetRetriveEvent damageUiGetRetrievEvent;

    public override void OnDamaged(int _damage)
    {
        DamageUI damageUI = damageUiGetRetrievEvent.RaiseGet();
        damageUI.Setting(uiPos);
        damageUI.ShowDamageText(_damage);
    }
}
