using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCreator : MonoBehaviour
{
    [SerializeField] private GameObject poisonPreb;

    private void Awake()
    {
        switch (GlobalData.curMapCode)
        {
            case 1:
                break;
            case 3:
                { 
                    Instantiate(poisonPreb, Vector3.zero, Quaternion.identity);
                }
                break;
            default:
                break;
        }
    }
}
