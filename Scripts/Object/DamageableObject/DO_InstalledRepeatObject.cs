using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//데미지를 주기적으로 주는 설치형 오브젝트 클래스 입니다.
public class DO_InstalledRepeatObject : DO_InstalledObject
{
    [SerializeField] private float damagecycleTime; //데미지 주기
    [SerializeField] private float damageTime;  //데미지 가용 시간

    private void OnEnable()
    {
        StartCoroutine(CoLogic());
    }

    private IEnumerator CoLogic()
    {
        float remainLiveTime = liveTime;
        float remainDamageTime = damageTime;

        do
        {
            yield return new WaitForSeconds(damagecycleTime);
            DealDamage();

            remainLiveTime -= damagecycleTime;
        }
        while ((remainDamageTime -= damagecycleTime) > 0);

        yield return new WaitForSeconds(remainLiveTime);

        objectPoolEvent.RaiseRelease(objectName, gameObject);
    }

    private void DealDamage()
    {
        Vector3 center = transform.position + Vector3.up * (boxSize.y * 0.5f); // 박스 중심 (지면 위쪽으로)
        Collider[] hits = Physics.OverlapBox(center, boxSize * 0.5f, Quaternion.identity, targetLayer);

        foreach (Collider col in hits)
        {
            Damageable target = col.GetComponent<Damageable>();
            if (target != null)
            {
                target.Damage(power);

                if (hitParticlePoolName != null)
                    target.PlayParticleOneShot(hitParticlePoolName, hitParParticleLocalPos, false, hitParticleSize);
            }
        }
    }
}
