using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//데미지를 받을 수 있는 오브젝트의 추상 클래스입니다.
//몬스터, 플레이어등 다양한 대상이 상속받아 구현합니다.
public abstract class Damageable : MonoBehaviour
{
    [SerializeField] private List<OnDamagedEventUser> onDamagedEventUsers;
    public event System.Action OnDamaged;
    public UnityEvent OnDead;

    //데미지 이벤트 수신자를 추가합니다.
    public void AddOnDamagedEventUser(OnDamagedEventUser _addTarget)
    { 
        onDamagedEventUsers.Add(_addTarget);
    }

    //데미지 이벤트 수신자를 제거합니다.
    public void RemoveOnDamagedEventUser(OnDamagedEventUser _removeTarget)
    {
        onDamagedEventUsers.Remove(_removeTarget);
    }

    //데미지 이벤트를 발생시킵니다.
    protected void RaiseDamageEvent(int _damage)
    {
        foreach (OnDamagedEventUser onDamagedEventUser in onDamagedEventUsers)
            onDamagedEventUser.OnDamaged(_damage);
    }

    //피해를 받습니다.
    public abstract void Damage(int _damage, bool _isDamageAction = true, bool _isIgnoreDef = false);

    //자신에게 물리적 힘을 받습니다.
    public abstract void Addforce(Vector3 _force);

    //전투불능 상태로 전환됩니다.
    public abstract void Dead();

    //버프를 적용합니다.
    public abstract void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility);

    //특정 카테고리의 버프를 제거합니다.
    public abstract void ClearBuff(List<Category> _categories);

    //자신에게 파티클을 지속적으로 재생합니다.
    public abstract void PlayParticleOn(string _particlePoolName, string _code, float _masterScale);

    //파티클 재생을 중단합니다.
    public abstract void PlayParticleOff(string _code);

    //자신에게 파티클을 1회 재생합니다.
    public abstract void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale);
}
