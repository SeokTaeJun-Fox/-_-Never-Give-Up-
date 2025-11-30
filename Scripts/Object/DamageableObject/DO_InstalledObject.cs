using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//설치형 데미지오브젝트입니다. (장판, 불기둥오브젝트등)
public class DO_InstalledObject : DamageableObject
{
    [SerializeField] protected Vector3 boxSize; // 감지 범위
    [SerializeField] protected float liveTime;  //생존시간
    [SerializeField] protected string hitParticlePoolName;
    [SerializeField] protected float hitParticleSize;
    [SerializeField] protected Vector3 hitParParticleLocalPos;
    protected float remainLiveTime;

    // 디버그용 박스 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + Vector3.up * (boxSize.y * 0.5f);
        Gizmos.DrawWireCube(center, boxSize);
    }
}
