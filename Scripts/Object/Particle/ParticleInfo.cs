using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInfo : MonoBehaviour
{
    [SerializeField] private string poolName;
    [SerializeField] private Vector3 localPos;

    public string PoolName { get => poolName; }
    public Vector3 LocalPos { get => localPos; }
}
