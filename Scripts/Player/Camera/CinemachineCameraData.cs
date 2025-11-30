using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CinemachineCameraData", menuName = "Scriptable Object/Settings/CinemachineCameraData")]
public class CinemachineCameraData : ScriptableObject
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float sensitivityMobile;

    public float Sensitivity { get => sensitivity; set => sensitivity = value; }
    public float SensitivityMobile { get => sensitivityMobile; set => sensitivityMobile = value; }
}
