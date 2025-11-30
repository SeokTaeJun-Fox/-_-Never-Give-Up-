using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInit : MonoBehaviour
{
    [SerializeField] private List<ResettableScriptableObject> resetableScriptableObjects;

    [SerializeField] private CinemachineCameraData cinemachineCameraData;
    [SerializeField] private string playerPrebName_rotateSen;

    private void Awake()
    {
        foreach (var reset in resetableScriptableObjects)
        {
            reset.Initial();

            cinemachineCameraData.Sensitivity = PlayerPrefs.GetFloat(playerPrebName_rotateSen, 500);
        }
    }
}
