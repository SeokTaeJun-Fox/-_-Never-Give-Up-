using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// 카메라 정보
    /// </summary>
    [SerializeField] private Camera realCamera;

    /// <summary>
    /// 따라가야할 트랜스폼
    /// </summary>
    [SerializeField] private Transform objectToFollow;

    /// <summary>
    /// 감도
    /// </summary>
    [SerializeField] private float sensitivity = 100f;

    /// <summary>
    /// 모바일 감도
    /// </summary>
    [SerializeField] private float sensitivityMobile = 30f;

    /// <summary>
    /// 각도 제한
    /// </summary>
    [SerializeField] private float clampAngleMax = 70f;

    /// <summary>
    /// 각도 제한
    /// </summary>
    [SerializeField] private float clampAngleMin = -10f;

    /// <summary>
    /// 최소거리
    /// </summary>
    [SerializeField] private float minDistance;

    /// <summary>
    /// 최대거리
    /// </summary>
    [SerializeField] private float maxDistance;

    /// <summary>
    /// PC , 유니티 에디터 감도
    /// </summary>
    [SerializeField] private float smoothness = 10f;

    /// <summary>
    /// 자동 회전 속도 (플레이어가 카메라기준으로 우회전/좌회전을 할경우 카메라가 자동으로 오른쪽으로/왼쪽으로 회전한다)
    /// </summary>
    [SerializeField] private float AutoRotatespeed;

    /// <summary>
    /// 인풋을 받을 변수(x)
    /// </summary>
    private float rotX;

    /// <summary>
    /// 인풋을 받을 변수(y)
    /// </summary>
    private float rotY;

    /// <summary>
    /// 방향
    /// </summary>
    private Vector3 dirNormalized;

    /// <summary>
    /// 최종 방향
    /// </summary>
    private Vector3 finalDir;

    /// <summary>
    /// 최종 거리
    /// </summary>
    private float finalDistance = 0;

    /// <summary>
    /// 레이어 무시
    /// </summary>
    private string ignoreLayer;

    /// <summary>
    /// 각도 제한 초깃값 (최솟값)
    /// </summary>
    private float originalClampAngleMin;

    /// <summary>
    /// 각도 제한 초깃값 (최댓값)
    /// </summary>
    private float originalClampAngleMax;

    //모바일 영역

    /// <summary>
    /// 오른손 터치ID - 카메라 회전 담당
    /// </summary>
    private int rightFingerId;

    /// <summary>
    /// 앞에 장애물이있을때 장애물하고 카메라사이의 거리
    /// </summary>
    [SerializeField] private float finalDirOffset;

    /// <summary>
    /// 최대 거리
    /// </summary>
    public float MaxDistance
    {
        get => maxDistance;
        set => maxDistance = value;
    }

    /// <summary>
    /// 각도 제한 설정(최솟값)
    /// </summary>
    public float SetClampAngleMin
    {
        set
        {
            clampAngleMin = value;
            ApplyCameraRotate();
        }
    }

    /// <summary>
    /// 각도 제한 설정(최댓값)
    /// </summary>
    public float SetClampAngleMax
    {
        set
        {
            clampAngleMax = value;
            ApplyCameraRotate();
        }
    }

    /// <summary>
    /// 카메라가 이 객체에 따라간다
    /// </summary>
    public Transform ObjectToFollow
    {
        set
        {
            objectToFollow = value;
        }
    }

    /// <summary>
    /// 무시할 레이어 설정
    /// </summary>
    public string SetIgnoreLayer
    {
        set => ignoreLayer = value;
    }

    #region function
    private void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.transform.localPosition.normalized;
        finalDistance = realCamera.transform.localPosition.magnitude;

        rightFingerId = -1; //-1은 추적중이 아닌 손가락

        //_playerManager.setCameraMovement = this;

        originalClampAngleMin = clampAngleMin;
        originalClampAngleMax = clampAngleMax;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR_WIN || UNITY_WEBGL
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

            ApplyCameraRotate();
        }
#endif
    }

    private void LateUpdate()
    {
        if (objectToFollow != null)
        {
            transform.position = objectToFollow.position; //Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

            finalDir = transform.TransformPoint(dirNormalized * maxDistance);

            //카메라와 오브젝트 사이의 장애물출력
            RaycastHit hit;

            if (Physics.Linecast(transform.position, finalDir, out hit, ~((1 << LayerMask.NameToLayer("Player")))))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer(ignoreLayer))
                    finalDistance = Mathf.Clamp(hit.distance + finalDirOffset, minDistance, maxDistance);
                else
                    finalDistance = maxDistance;
            }
            else
            {
                finalDistance = maxDistance;
            }

            //캐릭터와 카메라 사이의 거리변화
            realCamera.transform.localPosition = Vector3.Lerp(realCamera.transform.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        }
    }

    /// <summary>
    /// 카메라 방향을 적용한다
    /// </summary>
    private void ApplyCameraRotate()
    {
        rotX = Mathf.Clamp(rotX, clampAngleMin, clampAngleMax);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }
    #endregion
}
