using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//발사체 오브젝트 클래스입니다
public class DO_Projectile : DamageableObject
{
    [SerializeField] LayerMask collideLayer;    //특정 레이어에 부딫히면 폭발합니다.
    [SerializeField] private string explosionObj;   //폭발시 생성되는 오브젝트
    private Vector3 dir;    //방향
    private float spd;      //속도
    private float liveTime = 99; //생존시간

    private Coroutine coLiveTimer;

    public void Setting(int _power, Vector3 _dir, float _spd, float _liveTime, LayerMask _collideLayer, LayerMask _targetLayer)
    {
        this.collideLayer = _collideLayer;
        this.dir = _dir;
        this.spd = _spd;
        this.liveTime = _liveTime;

        base.Setting(_power, _targetLayer);

        coLiveTimer = StartCoroutine(CoLiveTimer());
    }

    private void Update()
    {
        transform.position += dir.normalized * spd * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //[other]가 데미지 줄수있는 타겟이라면 other오브젝트는 데미지를 입습니다.
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Debug.Log($"{other.gameObject.layer} : 데미지도");

            Damageable target = other.GetComponent<Damageable>();
            if (target != null)
            {
                target.Damage(power);
            }
            else
            {
                Debug.LogWarning($"{target}은 피해받을수있는 오브젝트가 아닙니다.");
            }

            StopCoroutine(coLiveTimer);

            //폭발처리
            objectPoolEvent.RaiseGet(explosionObj, transform.position, Quaternion.identity);     //폭발 오브젝트 생성(불러오기)
            objectPoolEvent.RaiseRelease(objectName, gameObject); //오브젝트 파괴(반환)

            return;
        }

        //[other]레이어를 닿을시 폭발할수 있다면 폭발 로직을 처리합니다.
        if (((1 << other.gameObject.layer) & collideLayer) != 0)
        {
            Debug.Log($"{other.gameObject.name} : 폭발만");

            StopCoroutine(coLiveTimer);

            //폭발처리
            objectPoolEvent.RaiseGet(explosionObj, transform.position, Quaternion.identity); //폭발 오브젝트 생성(불러오기)
            objectPoolEvent.RaiseRelease(objectName, gameObject); //오브젝트 파괴(반환)
        }
    }

    private IEnumerator CoLiveTimer()
    {
        yield return new WaitForSeconds(liveTime);
        objectPoolEvent.RaiseRelease(objectName, gameObject); //오브젝트 파괴(반환)
    }
}
