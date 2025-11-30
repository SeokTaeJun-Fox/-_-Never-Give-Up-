using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMonster : Damageable
{
    public override void Addforce(Vector3 _force)
    {

    }

    public override void Damage(int _damage, bool _isDamageAction = true, bool _isIgnoreDef = false)
    {
        Debug.Log($"name : {transform.name}, damage : {_damage}");
    }

    public override void Dead()
    {

    }

    public override void PlayParticleOff(string _code)
    {
    }

    public override void PlayParticleOn(string _particlePoolName, string _code, float _masterScale)
    {
    }

    public override void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale)
    {
    }

    public override void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {

    }

    public override void ClearBuff(List<Category> _categories)
    {

    }
}
