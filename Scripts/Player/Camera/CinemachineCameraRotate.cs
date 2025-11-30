using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CinemachineCameraRotate : MonoBehaviour
{
    /// <summary>
    /// 카메라가 바라봐야할 플레이어의 위치(트랜스폼)
    /// </summary>
    [SerializeField] private Transform objectToFollow;

    /// <summary>
    /// 설정 데이터 (민감도)
    /// </summary>
    [SerializeField] private CinemachineCameraData data;

    /// <summary>
    /// 각도 제한
    /// </summary>
    [SerializeField] private float clampAngleMax = 70f;

    /// <summary>
    /// 각도 제한
    /// </summary>
    [SerializeField] private float clampAngleMin = -10f;

    /// <summary>
    /// 자동 회전 속도 (플레이어가 카메라기준으로 우회전/좌회전을 할경우 카메라가 자동으로 오른쪽으로/왼쪽으로 회전한다)
    /// </summary>
    [SerializeField] private float AutoRotatespeed;

    //임시
    [SerializeField] private PlayerMovement pm;

    /// <summary>
    /// 인풋을 받을 변수(x)
    /// </summary>
    private float rotX;

    /// <summary>
    /// 인풋을 받을 변수(y)
    /// </summary>
    private float rotY;

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
    /// 카메라가 이 객체에 따라갑니다.
    /// </summary>
    public Transform ObjectToFollow
    {
        set
        {
            objectToFollow = value;
        }
    }

    #region function
    private void Start()
    {
        rotX = objectToFollow.rotation.eulerAngles.x;
        rotY = objectToFollow.rotation.eulerAngles.y;

        rightFingerId = -1; //-1은 추적중이 아닌 손가락

        originalClampAngleMin = clampAngleMin;
        originalClampAngleMax = clampAngleMax;

        //pm.OnRotate += OnRotate;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR_WIN || UNITY_WEBGL
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            rotX += -(Input.GetAxis("Mouse Y")) * data.Sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * data.Sensitivity * Time.deltaTime;

            ApplyCameraRotate();
        }
#elif UNITY_IOS || UNITY_ANDROID
            TouchAndDrag();
#endif
    }

    /// <summary>
    /// 모바일버전에서 터치앤 드래그시 3인칭 카메라회전
    /// </summary>
    private void TouchAndDrag()
    {
        //몇개의 터치가 입력되는가
        for (int touchId = 0; touchId < Input.touchCount; touchId++)
        {
            Touch touch = Input.GetTouch(touchId);

            switch (touch.phase)    //touchId번 "터치" 상태
            {
                case TouchPhase.Began:  //터치 시작했을때
                    {
                        //현재 카메라회전담당(터치id)이없고 ui위치에 없으면 그 "터치"를 카메라 회전담당이라고 한다
                        //각 터치를 구분할수있는 아이디가 있다
                        if (rightFingerId == -1
                            && !Utility.IsPointerOverUIObject(touch.position))//!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            rightFingerId = touch.fingerId;
                        }
                    }
                    break;

                case TouchPhase.Moved:  //드래그 하였을때
                    if (!Utility.IsPointerOverUIObject(touch.position))//EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        if (touch.fingerId == rightFingerId)
                        {
                            //Log.Instance.ShowLog1(touch.deltaPosition.ToString());
                            rotX += -touch.deltaPosition.y * data.SensitivityMobile * Time.deltaTime;
                            rotY += touch.deltaPosition.x * data.SensitivityMobile * Time.deltaTime;

                            rotX = Mathf.Clamp(rotX, clampAngleMin, clampAngleMax);

                            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
                            transform.rotation = rot;
                        }
                    }

                    break;

                case TouchPhase.Stationary: //멈췄을때
                    {
                        if (touch.fingerId == rightFingerId)
                        {
                        }
                    }
                    break;

                case TouchPhase.Ended:  //땠을때
                    {
                        if (touch.fingerId == rightFingerId)
                        {
                            rightFingerId = -1;
                        }
                    }
                    break;

                case TouchPhase.Canceled:   //전화오거나 백그라운드 상태로 놓아뒀을때
                    {
                        if (touch.fingerId == rightFingerId)
                        {
                            rightFingerId = -1;
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 카메라 방향을 적용한다
    /// </summary>
    private void ApplyCameraRotate()
    {
        rotX = Mathf.Clamp(rotX, clampAngleMin, clampAngleMax);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        objectToFollow.rotation = rot;
    }
    #endregion

    public void OnRotate(Vector3 inDir)
    {
        //플레이어가 회전할때 시네머신 추적 오브젝트도 회전이 된다.
        //플레이어가 회전할때 추적 오브젝트는 회전하지 말아야한다.
        //그래서 플레이어 회전 이벤트에 이 메소드를 등록하였다.
        rotX = Mathf.Clamp(rotX, clampAngleMin, clampAngleMax);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        objectToFollow.rotation = rot;
    }
}
