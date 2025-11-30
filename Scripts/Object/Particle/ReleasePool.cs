using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasePool : MonoBehaviour
{
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private float releaseTime;
    [SerializeField] private string poolName;
    private float remainTime;

    private void OnEnable()
    {
        remainTime = releaseTime;
    }

    private void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
        }
        else
        {
            poolEvent.RaiseRelease(poolName, gameObject);
        }
    }
}
