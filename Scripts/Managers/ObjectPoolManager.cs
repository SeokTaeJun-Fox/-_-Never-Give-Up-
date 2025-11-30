using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

//오브젝트 풀 관리자입니다.
public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPoolInfo> poolInfos;
    [SerializeField] private ObjectPoolEvent poolEvent;
    
    private Dictionary<string, MyObjectPool> poolInfoDic = new Dictionary<string, MyObjectPool>();

    private void Awake()
    {
        MyObjectPool objectPool;

        foreach (ObjectPoolInfo poolInfo in poolInfos)
        {
            objectPool = new MyObjectPool(poolInfo.prefab, poolInfo.defaultCapacity, poolInfo.maxCapacity, transform);
            poolInfoDic.Add(poolInfo.nameKey, objectPool);
        }

        if (poolEvent != null)
        {
            poolEvent.OnGet += GetObject;
            poolEvent.OnGet2 += GetObject;
            poolEvent.OnRelease += Release;
        }

        SceneManager.sceneUnloaded += OnUnloadScene;
    }

    public GameObject GetObject(string _nameKey)
    {
        if (poolInfoDic.ContainsKey(_nameKey))
        {
            return poolInfoDic[_nameKey].Get();
        }
        else
        {
            Debug.LogWarning($"{_nameKey}에 해당하는 오브젝트풀을 찾을 수 없습니다.");
            return null;
        }
    }

    public GameObject GetObject(string _nameKey, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject( _nameKey);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void Release(string nameKey, GameObject obj)
    {
        if (poolInfoDic.ContainsKey(nameKey))
        {
            poolInfoDic[nameKey].Release(obj);
        }
        else
        {
            Debug.LogWarning($"{nameKey}에 해당하는 오브젝트풀을 찾을 수 없습니다.");
        }
    }

    private void OnDestroy()
    {
        if (poolEvent != null)
        {
            poolEvent.OnGet -= GetObject;
            poolEvent.OnGet2 -= GetObject;
            poolEvent.OnRelease -= Release;
        }
    }

    public void OnUnloadScene(Scene scene)
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (var pool in poolInfoDic.Values)
        {
            pool.Reflash();
        }
    }
}

//오브젝트 풀 매니저에 사용하게될 클래스입니다.
//오브젝트 풀 생성시 필요한 정보가 들어있습니다.
[Serializable]
public class ObjectPoolInfo
{
    public GameObject prefab;
    public string nameKey;  //이름 (외부 클래스에서 키 값을 요청해 오브젝트를 불러옵니다)
    public int defaultCapacity;
    public int maxCapacity;
}
