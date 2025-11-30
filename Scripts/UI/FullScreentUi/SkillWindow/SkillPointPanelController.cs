using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointPanelController : MonoBehaviour
{
    [SerializeField] private SkillPointPanelView view;

    //이 클래스에 필요한 인터페이스들
    private ISkillManager skillManager;

    public void Initial(ISkillManager _skillManager)
    {
        skillManager = _skillManager;
    }

    public void OpenPanel()
    {
        if (skillManager != null)
        {
            view.ShowSkillPoint(skillManager.GetSkillPoint.ToString());
            skillManager.OnSkillPointUse += OnSkillPointUse;
        }
    }

    public void ClosePanel()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillPointUse -= OnSkillPointUse;
        }
    }

    //이벤트 처리

    private void OnSkillPointUse(int _remainSkillPoint)
    {
        view.ShowSkillPoint(_remainSkillPoint.ToString());
    }

    private void OnDestroy()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillPointUse -= OnSkillPointUse;
        }
    }
}
