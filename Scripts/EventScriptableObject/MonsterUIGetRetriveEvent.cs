using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 정보 ui를 불러오거나/회수할때 발생하는 이벤트 처리 클래스입니다.
[CreateAssetMenu(fileName = "MonsterUIGetRetriveEvent", menuName = "Scriptable Object/Event/MonsterUIGetRetriveEvent")]
public class MonsterUIGetRetriveEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>몬스터 정보UI 불러오기 요청시 실행됩니다.</summary>
    private Func<MonsterInfoUI> onGet;
    private Action<MonsterInfoUI> onRetrieve;

    public event Func<MonsterInfoUI> OnGet
    {
        add { onGet += value; }
        remove { onGet -= value; }
    }

    public event Action<MonsterInfoUI> OnRetrieve
    {
        add { onRetrieve += value; }
        remove { onRetrieve -= value; }
    }

    public MonsterInfoUI RaiseGet() => onGet?.Invoke();
    public void RaiseRetrieve(MonsterInfoUI _monsterInfoUI) => onRetrieve?.Invoke(_monsterInfoUI);

    public override void Initial()
    {
        onGet = null;
        onRetrieve = null;
    }
}
