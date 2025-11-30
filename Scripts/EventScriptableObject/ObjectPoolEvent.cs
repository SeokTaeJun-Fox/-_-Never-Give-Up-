using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 풀을 이용하여 객체를 Get/Release를 하는 이벤트 클래스입니다.
[CreateAssetMenu(fileName = "ObjectPoolEvent", menuName = "Scriptable Object/Event/ObjectPoolEvent")]
public class ObjectPoolEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>오브젝트 풀에서 객체를 가져올때 발생됩니다.</summary>
    private Func<string, GameObject> onGet;

    /// <summary>오브젝트 풀에서 객체를 가져올때 발생됩니다. (매개변수 위치/방향포함)</summary>
    private Func<string, Vector3, Quaternion, GameObject> onGet2;

    /// <summary>객체를 오브젝트 풀에 반환할때 발생됩니다.</summary>
    private Action<string, GameObject> onRelease;


    public event Func<string, GameObject> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public event Func<string, Vector3, Quaternion, GameObject> OnGet2
    {
        add { onGet2 += value; }
        remove { onGet2 -= value; }
    }

    public event Action<string, GameObject> OnRelease
    {
        add { onRelease += value; }
        remove { onRelease -= value; }
    }

    public GameObject RaiseGet(string _nameKey) => onGet?.Invoke(_nameKey);
    public GameObject RaiseGet(string _nameKey, Vector3 _pos, Quaternion _rot) => onGet2?.Invoke(_nameKey, _pos, _rot);
    public void RaiseRelease(string _nameKey, GameObject _releaseObject) => onRelease?.Invoke(_nameKey, _releaseObject);

    public override void Initial()
    {
        onGet = null;
        onRelease = null;
        onGet2 = null;
    }
}
