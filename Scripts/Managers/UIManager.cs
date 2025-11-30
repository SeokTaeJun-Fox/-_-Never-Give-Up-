using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 캔버스를 관리하고, 캔버스 요청 이벤트에 응답하며, 메인 UI를 활성화하는 관리자 클래스입니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Serializable]
    class CanvasLayerInfo
    {
        public Canvas canvas;   //캔버스 오브젝트
        public CanvasType canvasType;   //캔버스 타입
                                        //WorldLayer, MainLayer, DialogLayer, FullScreenLayer, AlwaysTopLayer
    }
    [SerializeField] private CanvasLayerInfo[] layerInfos;  //캔버스 레이어 정보 배열
    [SerializeField] private GetCanvasRequestEvent getCanvasRequestEvent;   //캔버스 요청 이벤트
    [SerializeField] private ActiveMainUIRequestEvent activeMainUIRequestEvent; //메인 UI 활성화 요청 이벤트
    private Dictionary<CanvasType, GameObject> canvasDic = new();   //캔버스 타입별 오브젝트 매핑

    //메인ui
    [SerializeField] private GameObject mainUIPreb; //메인 UI 프리팹
    private GameObject mainUI;  //생성된 메인 UI 인스턴스

    private void Awake()
    {
        //캔버스 정보를 딕셔너리에 등록
        foreach (CanvasLayerInfo layerInfo in layerInfos)
            canvasDic.Add(layerInfo.canvasType, layerInfo.canvas.gameObject);

        //이벤트 등록
        getCanvasRequestEvent.OnGet += GetCanvasObject;
        activeMainUIRequestEvent.OnActiveMainUIRequest += ActiveMainUI;
    }

    /// <summary>
    /// 캔버스 타입에 해당하는 오브젝트 반환
    /// </summary>
    /// <param name="_canvasType"></param>
    /// <returns></returns>
    public GameObject GetCanvasObject(CanvasType _canvasType)
    { 
        if (canvasDic.ContainsKey(_canvasType))
            return canvasDic[_canvasType];
        else
            return null;
    }

    /// <summary>
    /// 메인 UI를 활성화 또는 비활성화합니다.
    /// </summary>
    /// <param name="_isActive"></param>
    public void ActiveMainUI(bool _isActive)
    {
        if (_isActive)
        {
            if (mainUI == null)
            {
                Transform mainUIParent = GetCanvasObject(CanvasType.MainLayer)?.transform;
                mainUI = Instantiate(mainUIPreb, mainUIParent);
            }
            else
                mainUI.SetActive(true);
        }
        else
        { 
            if(mainUI != null)
                mainUI.SetActive(false);
        }
    }

    //이벤트
    private void OnDestroy()
    {
        //이벤트 해제
        getCanvasRequestEvent.OnGet -= GetCanvasObject;
        activeMainUIRequestEvent.OnActiveMainUIRequest -= ActiveMainUI;
    }
}
