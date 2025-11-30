using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3d 무기 착용시 발생하는 이벤트 스크립터블 오브젝트입니다.
[CreateAssetMenu(fileName = "Weapon3DChangedEvent", menuName = "Scriptable Object/Event/Weapon3DChangedEvent")]
public class Weapon3DChangedEvent : ResettableScriptableObject
{
    [TextArea, SerializeField] private string desc; //설명

    /// <summary>3d 무기 착용시 발생됩니다.</summary>
    private Action<GameObject> onWeaponChanged;

    public event Action<GameObject> OnWeaponChanged
    {
        add { onWeaponChanged += value; }
        remove { onWeaponChanged -= value; }
    }

    public void Raise(GameObject _weapon) => onWeaponChanged?.Invoke(_weapon);

    public override void Initial()
    {
        onWeaponChanged = null;
    }
}
