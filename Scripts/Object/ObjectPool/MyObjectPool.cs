using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//오브젝트 풀 클래스입니다 (1클래스당 1종류의 객체 관리를 합니다.)
public class MyObjectPool
{
    private ObjectPool<GameObject> pool;
    private GameObject prefab;
    private Transform parent;

    private int defaultCapacity;
    private int maxSize;

    public MyObjectPool(GameObject _prefab, int _defaultCapacity, int _maxSize, Transform _parent)
    {
        prefab = _prefab;
        defaultCapacity = _defaultCapacity;
        maxSize = _maxSize;
        parent = _parent;

        //오브젝트 풀 생성
        pool = new ObjectPool<GameObject>(
            OnCreate,   //오브젝트 생성시 실행
            OnGet,      //오브젝트 가져올시 실행
            OnRelease,  //오브젝트 반환할시 실행
            OnDestroyObject,    //오브젝트 파괴시 실행
            false,      //중복체크
            _defaultCapacity,   //기본 생성량
            _maxSize        //최대 생성량
            );
    }

    private GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(prefab);
        return go;
    }

    private void OnGet(GameObject _go)
    {
        _go.SetActive(true);
    }

    private void OnRelease(GameObject _go)
    {
        _go.SetActive(false);
    }

    private void OnDestroyObject(GameObject _go)
    {
        Debug.Log($"{_go} : 파괴...");
        GameObject.Destroy(_go);
    }

    //public
    public GameObject Get()
    {
        return pool.Get();
    }

    public void Release(GameObject _go)
    {
        pool.Release(_go);
    }

    public void Reflash()
    {
        pool.Clear();

        pool = new ObjectPool<GameObject>(
        OnCreate,   //오브젝트 생성시 실행
        OnGet,      //오브젝트 가져올시 실행
        OnRelease,  //오브젝트 반환할시 실행
        OnDestroyObject,    //오브젝트 파괴시 실행
        false,      //중복체크
        defaultCapacity,   //기본 생성량
        maxSize        //최대 생성량
        );
    }
}
