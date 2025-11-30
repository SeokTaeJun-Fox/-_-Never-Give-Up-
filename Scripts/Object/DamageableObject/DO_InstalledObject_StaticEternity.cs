using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DO_InstalledObject_StaticEternity : DamageableObject
{
    public float poisonInterval = 2f; // 버프 반복 간격 (초)
    private Coroutine poisonCoroutine;
    [SerializeField] private Buff takeBuff;

    private Dictionary<PlayerStat, object> stat = new Dictionary<PlayerStat, object>();

    private void Awake()
    {
        stat.Add(PlayerStat.TOTAL_ATK, power);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable target = other.GetComponent<Damageable>();
            if (target == null)
                return;

            // 즉시 버프 적용
            ApplyPoison(target);

            // 반복 버프 시작
            poisonCoroutine = StartCoroutine(ApplyPoisonOverTime(target));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 반복 버프 중단
            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine = null;
            }
        }
    }

    private void ApplyPoison(Damageable player)
    {
        Debug.Log("틱");
        player.TakeBuff(takeBuff, stat);
    }

    private IEnumerator ApplyPoisonOverTime(Damageable player)
    {
        while (true)
        {
            yield return new WaitForSeconds(poisonInterval);
            ApplyPoison(player);
        }
    }

}
