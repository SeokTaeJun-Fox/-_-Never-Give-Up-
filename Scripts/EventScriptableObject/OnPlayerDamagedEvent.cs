using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 데미지를 입혔을시 발생되는 이벤트 스크립터블 오브젝트 입니다. (이 이벤트는 플레이어매니저를 거칩니다)
[CreateAssetMenu(fileName = "OnPlayerDamagedEvent", menuName = "Scriptable Object/Event/OnPlayerDamagedEvent")]
public class OnPlayerDamagedEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    private Action<int> onPlayerDamaged;


    public event Action<int> OnPlayerDamaged
    {
        add { onPlayerDamaged += value; }
        remove { onPlayerDamaged -= value; }
    }

    public void RaiseEvent(int _count) => onPlayerDamaged?.Invoke(_count);

    public override void Initial()
    {
        onPlayerDamaged = null;
    }
}
