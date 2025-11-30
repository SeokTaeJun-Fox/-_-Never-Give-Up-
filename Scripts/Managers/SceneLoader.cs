using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

public class SceneLoader : MonoBehaviour, ISceneLoader
{
    [SerializeField] private SceneLoaderRegistry registry;

    public event Action<float> OnProgressChanged;
    public event Action OnSceneLoaded;
    public event Action OnLoadComplete;
    public float Progress => progress;

    //이벤트 스크립터블 오브젝트
    [SerializeField] private FadeRequestEvent fadeRequestEvent;
    [SerializeField] private LoadSceneRequestEvent loadSceneRequestEvent;
    [SerializeField] private ShowUIWindowFactoryEvent showUIWindowFactoryEvent;

    private int curLoadSceneIndex;
    private float progress;
    private bool isLoading;

    private void Awake()
    {
        ServiceLocator.Register<ISceneLoader>(this);

        if(loadSceneRequestEvent != null)
            loadSceneRequestEvent.OnSceneLoadRequest += LoadScene;
    }

    public void LoadScene(LoadSceneType _sceneType, int _mapCode)
    {
        if (isLoading)
            return;

        isLoading = true;
        OnSceneLoaded?.Invoke();
        GlobalData.curMapCode = _mapCode;
        GlobalData.curScene = _sceneType;

        SceneLoaderEntry entry = registry.Entry.Find(x => x.SceneType == _sceneType);
        if (entry != null)
        {
            curLoadSceneIndex = entry.BuildSceneIndex;

            if (fadeRequestEvent != null)
            {
                fadeRequestEvent.RaiseFadeIn(LoadSceneProcess);
            }
            else
            {
                LoadSceneProcess();
            }
        }
        else
        {
            Debug.LogError($"{_sceneType}에 해당하는 씬 정보를 불러오지 못했습니다.");
        }
    }

    private void LoadSceneProcess()
    {
        StartCoroutine(CoLoadSceneProcess());
    }

    private IEnumerator CoLoadSceneProcess()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(curLoadSceneIndex);

        if (showUIWindowFactoryEvent != null)
            showUIWindowFactoryEvent.Raise(UIType.LOADING_WINDOW);

        progress = 0;
        OnProgressChanged?.Invoke(progress);

        yield return new WaitForSeconds(0.2f);

        while (!ao.isDone)
        { 
            yield return null;

            progress = ao.progress;
            OnProgressChanged?.Invoke(progress);
        }

        progress = 1;
        OnProgressChanged?.Invoke(progress);

        OnLoadComplete?.Invoke();

        isLoading = false;
        yield return new WaitForSeconds(1f);

        if (fadeRequestEvent != null)
        {
            fadeRequestEvent.RaiseFadeOut(() => { });
        }
    }

    //이벤트
    private void OnDestroy()
    {
        if (loadSceneRequestEvent != null)
            loadSceneRequestEvent.OnSceneLoadRequest -= LoadScene;
    }
}
