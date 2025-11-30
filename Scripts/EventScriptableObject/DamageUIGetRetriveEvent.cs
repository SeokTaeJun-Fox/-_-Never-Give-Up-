using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//데미지 ui를 불러오거나/회수할때 발생하는 이벤트 처리 클래스입니다.
[CreateAssetMenu(fileName = "DamageUIGetRetriveEvent", menuName = "Scriptable Object/Event/DamageUIGetRetriveEvent")]
public class DamageUIGetRetriveEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>데미지UI 불러오기 요청시 실행됩니다.</summary>
    private Func<DamageUI> onGet;
    private Action<DamageUI> onRetrieve;

    public event Func<DamageUI> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public event Action<DamageUI> OnRetrieve
    {
        add { onRetrieve += value; }
        remove { onRetrieve -= value; }
    }

    public DamageUI RaiseGet() => onGet?.Invoke();
    public void RaiseRetrieve(DamageUI _damageUI) => onRetrieve?.Invoke(_damageUI);

    public override void Initial()
    {
        onGet = null;
        onRetrieve = null;
    }
}
