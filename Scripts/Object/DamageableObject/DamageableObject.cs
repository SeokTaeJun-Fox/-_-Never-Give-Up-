using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//데미지를 주는 오브젝트클래스 입니다.
public class DamageableObject : MonoBehaviour
{
    [SerializeField] protected ObjectPoolEvent objectPoolEvent;
    [SerializeField] protected string objectName;   //오브젝트 풀에 사용됩니다.

    [SerializeField] protected int power;  //공격력
    [SerializeField] protected LayerMask targetLayer;

    public virtual void Setting(int _power, LayerMask _targetLayer)
    { 
        power = _power;
        targetLayer = _targetLayer;
    }
}
