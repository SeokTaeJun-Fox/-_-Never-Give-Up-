using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalPanel : MonoBehaviour, IUIWindow
{
    [SerializeField] private PortalPanelElementView preb;
    [SerializeField] private Transform prebParent;
    [SerializeField] private List<PortalPanelElementView> cache;
    private PortalPanelData panelData;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;
    [SerializeField] private UnityEvent elementButtonEvent;

    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private LoadSceneRequestEvent loadSceneRequest;
    [SerializeField] private ReportOpenWindowEvent openEvent;

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is PortalPanelData ppd)
                panelData = ppd;
        }
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        Setting();
        onOpen?.Invoke();
    }

    private void Setting()
    {
        int index = 0;

        foreach (var data in panelData.Datas)
        {
            var inData = data;

            if (index < cache.Count)
            {
               
                cache[index].gameObject.SetActive(true);
                cache[index].SetText(data.ButtonText);
                cache[index].Button.interactable = data.IsCondition;

                cache[index].Button.onClick.RemoveAllListeners();
                cache[index].Button.onClick.AddListener(() =>
                {
                    openEvent?.RaiseCloseWindow();
                    elementButtonEvent?.Invoke();
                    Close();
                    LoadScene(inData.LoadSceneType, inData.MapId);
                });
            }
            else
            {
                var newElement = Instantiate(preb, prebParent);
                newElement.gameObject.SetActive(true);
                newElement.SetText(data.ButtonText);
                newElement.Button.interactable = data.IsCondition;
                cache.Add(newElement);

                newElement.Button.onClick.RemoveAllListeners();
                newElement.Button.onClick.AddListener(() =>
                {
                    openEvent?.RaiseCloseWindow();
                    elementButtonEvent?.Invoke();
                    Close();
                    LoadScene(inData.LoadSceneType, inData.MapId);
                });
            }

            index++;
        }
    }

    public void Close()
    {
        foreach(var element in cache)
            element.gameObject.SetActive(false);

        panelData = null;
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    private void LoadScene(LoadSceneType _type, int _mapId)
    { 
        loadSceneRequest.Raise(_type, _mapId);
    }

    public void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
