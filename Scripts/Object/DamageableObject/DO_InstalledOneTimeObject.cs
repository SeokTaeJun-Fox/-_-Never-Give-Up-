using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//데미지를 단 1번만 주는 설치형 오브젝트 클래스 입니다.
public class DO_InstalledOneTimeObject : DO_InstalledObject
{
    [SerializeField] private float damageTiming;    //몇 초뒤에 대상에게 피해를 주는지 설정합니다.

    [SerializeField] private UnityEvent onDeal;

    private void OnEnable()
    {
        StartCoroutine(CoLogic());
    }

    private IEnumerator CoLogic()
    { 
        yield return new WaitForSeconds(damageTiming);

        DealDamage();

        yield return new WaitForSeconds(liveTime - damageTiming);

        objectPoolEvent.RaiseRelease(objectName, gameObject);
    }

    private void DealDamage()
    {
        onDeal?.Invoke();
        Vector3 center = transform.position + Vector3.up * (boxSize.y * 0.5f); // 박스 중심 (지면 위쪽으로)
        Collider[] hits = Physics.OverlapBox(center, boxSize * 0.5f, Quaternion.identity, targetLayer);

        foreach (Collider col in hits)
        {
            Damageable target = col.GetComponent<Damageable>();
            if (target != null)
            {
                target.Damage(power);

                if(hitParticlePoolName != null)
                    target.PlayParticleOneShot(hitParticlePoolName, hitParParticleLocalPos, false, hitParticleSize);
            }
        }
    }
}
