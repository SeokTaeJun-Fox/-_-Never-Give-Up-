using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Damageable 클래스의 OnDamage이벤트 사용자 클래스입니다. (이 클래스는 Damageable클래스의 이벤트를 사용할수 있습니다.)
public abstract class OnDamagedEventUser : MonoBehaviour
{
    public abstract void OnDamaged(int _damage);
}
