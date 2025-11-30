using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyParent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
