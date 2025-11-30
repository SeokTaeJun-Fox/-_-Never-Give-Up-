using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillHotKeyInfo", menuName = "Scriptable Object/PlayerSkill/ETC/SkillHotKeyInfo")]
public class SkillHotKeyInfo : ResettableScriptableObject
{
    [SerializeField] private List<SkillAndHotKeyInfo> infos;

    public event Action<int, Skill> OnChangeSkill;
    public List<SkillAndHotKeyInfo> Infos => infos;

    //핫키에 저장된 스킬을 불러옵니다.
    public Skill GetSkill(int _keyIndex)
    {
        if (infos.Count <= _keyIndex)
            return null;
        else
            return infos[_keyIndex].skill;
    }

    public void SetSkill(int _keyIndex, Skill _skill)
    {
        //Skill InstantiateSkill = (_skill != null) ? Instantiate(_skill) : null;
        if(_skill != null) _skill.IsModifyRemainCoolTime = true;

        if (infos.Count > _keyIndex)
            infos[_keyIndex].skill = _skill;

        OnChangeSkill?.Invoke(_keyIndex, _skill);
    }

    public void ResetEvent()
    {
        OnChangeSkill = null;
    }

    public void ClearData()
    {
        foreach(var info in infos)
            info.skill = null;

        for (int i = 0; i < infos.Count; i++)
        {
            OnChangeSkill?.Invoke(i, null);
        }
    }

    public override void Initial()
    {
        ResetEvent();
        ClearData();
    }
}

[System.Serializable]
public class SkillAndHotKeyInfo
{
    public KeyCode key;
    public Skill skill;
}
