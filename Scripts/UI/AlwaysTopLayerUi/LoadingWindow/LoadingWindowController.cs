using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private LoadingWindowView view;

    //이 클래스에 필요한 인터페이스들
    private ISceneLoader sceneLoader;

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is ISceneLoader isl)
                this.sceneLoader = isl;
        }
    }

    public void Initial(object[] _datas)
    {
        if (sceneLoader != null)
        {
            sceneLoader.OnProgressChanged += OnProgressChanged;
            sceneLoader.OnLoadComplete += OnLoadCompleted;

            view.InitialPanelSize();
            SetViewText(sceneLoader.Progress);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (sceneLoader != null)
        {
            sceneLoader.OnProgressChanged -= OnProgressChanged;
            sceneLoader.OnLoadComplete -= OnLoadCompleted;
        }
    }

    private void SetViewText(float _progress)
    {
        view.SetView(_progress);
    }

    //이벤트
    private void OnProgressChanged(float _progress)
    {
        SetViewText(_progress);
    }

    private void OnLoadCompleted()
    {
        view.EffectDisappear(Close);
    }

    private void OnDestroy()
    {
        if (sceneLoader != null)
        {
            sceneLoader.OnProgressChanged -= OnProgressChanged;
            sceneLoader.OnLoadComplete -= OnLoadCompleted;
        }
    }
}
