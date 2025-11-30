using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillListElementController : MonoBehaviour
{
    [SerializeField] private SkillListElementView view;

    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;

    //이 클래스에 필요한 인터페이스들
    private ISkillManager skillManager;

    public event Action<SkillListElementController> OnClickPanelButton;   //패널 클릭시 이벤트가 발생됩니다.
    private string targetSkillId;
    private Skill targetSkill;

    public string TargetSkillId => targetSkillId;

    public void Initial(ISkillManager _skillManager)
    { 
        skillManager = _skillManager;

        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp += OnSkillLevelUp;
            skillManager.OnSkillPointUse += OnSkillPointUse;
        }

        view.LevelUpButton.onClick.AddListener(OnClickLevelUpButton);
        view.KeyRegisterButton.onClick.AddListener(OnClickKeyRegisterButton);
        view.PanelButton.onClick.AddListener(OnClickPanel);
    }

    public void Setting(string _skillId)
    {
        targetSkillId = _skillId;
        this.targetSkill = null;

        if (skillManager != null)
        {
            //현재 스킬을 불러옵니다.
            IReadOnlyDictionary<string, Skill> curSkills = skillManager.GetCurrentSkill();

            //스킬 번들에 이 스킬아이디가 존재한다면
            if (curSkills.ContainsKey(_skillId))
            {
                Skill targetSkill = curSkills[_skillId];
                this.targetSkill = targetSkill;

                //이 스킬이 현재 플레이어가 보유하고있다면
                if (targetSkill != null)
                    view.ShowView(targetSkill.SkillSprite, targetSkill.SkillName, targetSkill.SkillLevel);
                //그렇지 않다면 id에 해당하는 스킬을 불러옵니다 (ui를 보여주기위해서 입니다.)
                else
                {
                    Skill sampleSkill = skillManager.GetSkill(_skillId, 1);
                    if (sampleSkill != null)
                    {
                        view.ShowView(sampleSkill.SkillSprite, sampleSkill.SkillName, 0);
                    }
                }
            }

            //버튼 활성화 확인
            CheckLevelUpButtonInteractable();
            CheckKeyRegisterButtonInteractable();
        }
    }

    public void Select(bool _isSelected)
    {
        if (!view.IsSelected && _isSelected)
        {
            view.ShowSelectedPanel(true);
            OnClickPanelButton?.Invoke(this);
        }
        else if (view.IsSelected && !_isSelected)
        {
            view.ShowSelectedPanel(false);
        }
    }

    public void ResetPanel()
    {
        targetSkillId = string.Empty;
        targetSkill = null;
        view.Initial();
    }

    private void CheckLevelUpButtonInteractable()
    {
        if (skillManager != null)
        {
            //현재 스킬을 불러옵니다.
            IReadOnlyDictionary<string, Skill> curSkills = skillManager.GetCurrentSkill();

            if (skillManager.GetSkillPoint == 0)
            {
                view.LevelUpButton.interactable = false;
                return;
            }

            if (!curSkills.ContainsKey(targetSkillId))
            {
                view.LevelUpButton.interactable = false;
                return;
            }

            if (curSkills[targetSkillId] == null ||
                curSkills[targetSkillId].SkillLevel < skillManager.GetMaxLevel(targetSkillId))
            {
                view.LevelUpButton.interactable = true;
            }
            else
            {
                view.LevelUpButton.interactable = false;
            }
        }
    }

    private void CheckKeyRegisterButtonInteractable()
    {
        if (skillManager != null)
        {
            //현재 스킬을 불러옵니다.
            IReadOnlyDictionary<string, Skill> curSkills = skillManager.GetCurrentSkill();

            if (!curSkills.ContainsKey(targetSkillId))
            {
                view.KeyRegisterButton.interactable = false;
                return;
            }

            if (curSkills[targetSkillId] != null)
            {
                view.KeyRegisterButton.interactable = true;
            }
            else
            {
                view.KeyRegisterButton.interactable = false;
            }
        }
    }

    //이벤트 처리
    private void OnEnable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp += OnSkillLevelUp;
            skillManager.OnSkillPointUse += OnSkillPointUse;
        }
    }

    private void OnDisable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp -= OnSkillLevelUp;
            skillManager.OnSkillPointUse -= OnSkillPointUse;
        }
    }

    private void OnDestroy()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLevelUp -= OnSkillLevelUp;
            skillManager.OnSkillPointUse -= OnSkillPointUse;
        }
    }

    private void OnSkillLevelUp(Skill _skill)
    {
        if (_skill == null)
            return;

        if(_skill.SkillId == targetSkillId)
            Setting(_skill.SkillId);
    }

    private void OnSkillPointUse(int _point)
    {
        if (_point == 0)
            view.LevelUpButton.interactable = false;
    }

    private void OnClickLevelUpButton()
    {
        if (skillManager != null)
        {
            skillManager.SkillLevelup(targetSkillId);
        }
    }

    private void OnClickKeyRegisterButton()
    {
        if (factoryEvent != null && skillManager != null)
        {
            factoryEvent.Raise(UIType.SKILL_SLOT_WINDOW, new object[] { targetSkill });
        }
    }

    private void OnClickPanel()
    {
        Select(true);
    }
}
