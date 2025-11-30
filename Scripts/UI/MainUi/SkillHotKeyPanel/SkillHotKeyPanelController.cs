using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHotKeyPanelController : MainUIElement
{
    //이 클래스에서 필요한 인터페이스들
    ISkillManager skillManager;
    
    //이 클래스에서 필요한 스크립터블 오브젝트들
    [SerializeField] private SkillHotKeyInfo hotKeyInfo;

    [SerializeField] private List<SkillHotKeyPanelElementController> elements;

    private bool isStop;

    public override void Initial()
    {
        if (hotKeyInfo != null)
        {
            for (int index = 0; index < elements.Count; index++)
            { 
                Skill skill = hotKeyInfo.GetSkill(index);
                elements[index].Setting(skill);
            }
        }

        hotKeyInfo.OnChangeSkill += OnChangeSkill;

        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp += OnSkillLevelUp;
        }
    }

    public override void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is ISkillManager sm)
                skillManager = sm;
        }
    }

    //이벤트 처리
    private void OnChangeSkill(int _index, Skill _skill)
    {
        if (elements.Count > _index)
            elements[_index].Setting(_skill);
    }

    private void OnDestroy()
    {
        if(hotKeyInfo != null)
            hotKeyInfo.OnChangeSkill -= OnChangeSkill;

        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp -= OnSkillLevelUp;
        }
    }

    private void OnSkillLevelUp(Skill _skill)
    {
        if (hotKeyInfo != null && _skill != null)
        {
            int findIndex = hotKeyInfo.Infos.FindIndex((x) => (x.skill != null) && (x.skill.SkillId == _skill.SkillId));
            
            if(findIndex != -1)
                hotKeyInfo.SetSkill(findIndex, _skill);
        }
    }
}
