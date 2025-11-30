using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//시네머신 카메라를 세팅하는 클래스입니다.
public class PlayerCinemachineCameraSetting : MonoBehaviour
{
    /// <summary>
    /// 시네머신 버츄얼 카메라
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera cvCamera;

    public void Setting(Transform _followObject)
    {
        cvCamera.Follow = _followObject;
        cvCamera.LookAt = _followObject;
    }
}
