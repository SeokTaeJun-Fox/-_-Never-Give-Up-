using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CinemachineCameraZoom : MonoBehaviour
{
    /// <summary>
    /// 확대 최소값
    /// </summary>
    [SerializeField] private float minView;

    /// <summary>
    /// 확대 최대값
    /// </summary>
    [SerializeField] private float maxView;

    /// <summary>
    /// 확대 초깃값
    /// </summary>
    [SerializeField] private float initView;

    /// <summary>
    /// 감도(PC)
    /// </summary>
    [SerializeField] private float sensitivity = 40f;

    /// <summary>
    /// 감도 (Android)
    /// </summary>
    [SerializeField] private float sensitivityMobile = 100f;

    /// <summary>
    /// 시네머신 버츄얼 카메라
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera cvCamera;

    /// <summary>
    /// 터치ID - 카메라 확대/축소 담당1
    /// </summary>
    private int zoomTouchId1 = -1;

    /// <summary>
    /// 터치ID - 카메라 화갣/축소 담당2
    /// </summary>
    private int zoomTouchId2 = -1;

    /// <summary>
    /// 멀티터치중 터치1
    /// </summary>
    private Touch touchZero;

    /// <summary>
    /// 멀티터치중 터치2
    /// </summary>
    private Touch touchOne;

    /// <summary>
    /// 최대 줌 값
    /// </summary>
    public float MaxView => maxView;

    private void Awake()
    {
        if (cvCamera != null)
        {
            cvCamera.m_Lens.FieldOfView = initView;
        }
    }

    private void Update()
    {
        Zoom();
    }

    /// <summary>
    /// 카메라 확대/축소
    /// </summary>
    private void Zoom()
    {
#if UNITY_STANDALONE || UNITY_EDITOR_WIN

        if (cvCamera != null)
        {
            cvCamera.m_Lens.FieldOfView += -Input.GetAxis("Mouse ScrollWheel") * sensitivity * Time.deltaTime;
            cvCamera.m_Lens.FieldOfView = Mathf.Clamp(cvCamera.m_Lens.FieldOfView, minView, maxView);
#elif UNITY_ANDROID
            //몇개의 터치가 입력되는가
            for (int touchId = 0; touchId < Input.touchCount; touchId++)
            {
                Touch touch = Input.GetTouch(touchId);

                switch (touch.phase)    //touchId번 "터치" 상태
                {
                    case TouchPhase.Began:
                        {
                            if (!Utility.IsPointerOverUIObject(touch.position)
                                && zoomTouchId1 == -1)
                            {
                                zoomTouchId1 = touch.fingerId;
                            }
                            else if (!Utility.IsPointerOverUIObject(touch.position)
                                    && zoomTouchId2 == -1)
                            {
                                zoomTouchId2 = touch.fingerId;
                            }
                        }
                        break;

                    case TouchPhase.Moved:
                        break;

                    case TouchPhase.Stationary:
                        break;

                    case TouchPhase.Ended:
                        {
                            if (touch.fingerId == zoomTouchId1)
                                zoomTouchId1 = -1;
                            else if (touch.fingerId == zoomTouchId2)
                                zoomTouchId2 = -1;
                        }
                        break;

                    case TouchPhase.Canceled:
                        {
                            if (touch.fingerId == zoomTouchId1)
                                zoomTouchId1 = -1;
                            else if (touch.fingerId == zoomTouchId2)
                                zoomTouchId2 = -1;
                        }
                        break;
                }
            }

            if (zoomTouchId1 != -1 && zoomTouchId2 != -1)
            {
                touchZero = getTouch(zoomTouchId1);
                touchOne = getTouch(zoomTouchId2);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                cvCamera.m_Lens.FieldOfView += deltaMagnitudeDiff * sensitivityMobile * Time.deltaTime;
                cvCamera.m_Lens.FieldOfView = Mathf.Clamp(cvCamera.m_Lens.FieldOfView, minView, maxView);
            }
#endif
        }
    }

    /// <summary>
    /// id에 해당하는 터치 구조체를 가져온다
    /// </summary>
    /// <param name="inFingerId">id</param>
    /// <returns></returns>
    private Touch getTouch(int inFingerId)
    {
        Touch t = Input.GetTouch(0);

        for (int index = 0; index < Input.touchCount; index++)
        {
            t = Input.GetTouch(index);
            if (t.fingerId == inFingerId)
                return t;
        }

        return t;
    }
}
