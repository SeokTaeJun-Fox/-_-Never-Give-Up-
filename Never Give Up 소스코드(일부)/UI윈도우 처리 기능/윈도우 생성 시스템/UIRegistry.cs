using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UIEntry들을 등록하고 관리하는 ScriptableObject입니다.
// UIWindowFactory에서 UIEntry를 조회할 때 사용됩니다.
[CreateAssetMenu(fileName = "UIRegistry", menuName = "Scriptable Object/UI/UIRegistry")]
public class UIRegistry : ScriptableObject
{
    [SerializeField] private List<UIEntry> entries; //등록된 UIEntry 리스트

    public List<UIEntry> Entries { get => entries; }
}
