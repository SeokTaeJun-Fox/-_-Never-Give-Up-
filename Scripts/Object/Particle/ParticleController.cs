using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//파티클 컨트롤러 클래스입니다.
public class ParticleController : MonoBehaviour
{
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private string objectName; //오브젝트 풀에 사용됩니다.
    [SerializeField] private float liveTime;

    private Coroutine coLiveTimer;

    private void OnEnable()
    {
        coLiveTimer = StartCoroutine(CoLiveTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(coLiveTimer);
    }

    private IEnumerator CoLiveTimer()
    {
        yield return new WaitForSeconds(liveTime);
        poolEvent.RaiseRelease(objectName, gameObject);
    }
}
