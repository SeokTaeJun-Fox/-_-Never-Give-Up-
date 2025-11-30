using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//스킬 단축키용 패널을 구성하고있는 요소ui 컨트롤러 입니다.
public class SkillHotKeyPanelElementController : MonoBehaviour
{
    [SerializeField] private SkillHotKeyPanelElementView view;

    private Skill target;
    public Skill Target => target;

    private bool isStop;

    private void Update()
    {
        if (!isStop && target != null && target.IsModifyRemainCoolTime)
        {
            if(target.RemainCoolTime > 0)
                target.RemainCoolTime -= Time.deltaTime;

            view.SetSlice(target.RemainCoolTime / target.SkillCooltime);
        }
    }

    public void Setting(Skill _skill)
    {
        target = _skill;

        if (_skill != null)
        {
            view.ShowView(_skill.SkillSprite);
            view.SetSlice(_skill.RemainCoolTime / _skill.SkillCooltime);
        }
        else
            view.ShowView(null);
    }

    public void Stop()
    {
        isStop = true;    
    }

    public void Resume()
    {
        isStop = false;
    }
}
