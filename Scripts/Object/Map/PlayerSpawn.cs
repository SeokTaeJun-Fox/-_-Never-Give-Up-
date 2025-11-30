using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private CreatePlayerRequestEvent e;

    private void Start()
    {
        e.RaiseEnter(transform.position, transform.rotation);
    }
}
