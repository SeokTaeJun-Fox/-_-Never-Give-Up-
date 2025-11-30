using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDescPanelController : MonoBehaviour
{
    [SerializeField] private SkillDescPanelView view;

    //이 클래스에 필요한 인터페이스들
    private ISkillManager skillManager;
    private string targetSkillId;

    public void Initial(ISkillManager _skillManager)
    {
        skillManager = _skillManager;
    }

    public void OpenPanel()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp += OnLevelUp;
            view.Hide();
        }
    }

    public void ClosePanel()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp -= OnLevelUp;
        }
    }

    public void Setting(string _skillId)
    {
        if (skillManager == null)
            return;

        targetSkillId = _skillId;

        IReadOnlyDictionary<string, Skill> mySkillDic = skillManager.GetCurrentSkill();

        if (!mySkillDic.ContainsKey(_skillId))
        {
            view.Hide();
            return;
        }

        if (mySkillDic[_skillId] == null)
        {
            Skill nextSkill = skillManager.GetSkill(_skillId, 1);
            if (nextSkill != null)
            {
                view.ShowNextSkillDesc(nextSkill.SkillName, nextSkill.MpCost.ToString(), nextSkill.SkillDesc);
            }
            else
            {
                view.Hide();
            }

            return;
        }

        if (mySkillDic[_skillId].SkillLevel < skillManager.GetMaxLevel(_skillId))
        {
            Skill curSkill = mySkillDic[_skillId];
            Skill nextSkill = skillManager.GetSkill(_skillId, curSkill.SkillLevel + 1);

            if (nextSkill != null)
            {
                view.ShowCurNextSkillDesc(curSkill.SkillName, curSkill.MpCost.ToString(), curSkill.SkillDesc, nextSkill.SkillName, nextSkill.MpCost.ToString(), nextSkill.SkillDesc);
            }
            else
            {
                view.Hide();
                Debug.LogError($"{curSkill.name}스킬 {curSkill.SkillLevel+1}레벨이 존재하지 않습니다.");
            }

            return;
        }

        if (mySkillDic[_skillId].SkillLevel == skillManager.GetMaxLevel(_skillId))
        {
            Skill curSkill = mySkillDic[_skillId];
            view.ShowCurSkillDesc(curSkill.SkillName, curSkill.MpCost.ToString(), curSkill.SkillDesc);
            return;
        }
    }

    //이벤트 처리

    private void OnDestroy()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp -= OnLevelUp;
        }
    }

    private void OnLevelUp(Skill _skill)
    {
        if (targetSkillId == _skill.SkillId)
        {
            Setting(_skill.SkillId);
        }
    }
}
