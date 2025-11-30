using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MySkillPanelController : MonoBehaviour
{
    [SerializeField] private SkillListElementController elementPreb;
    [SerializeField] private Transform prebParent;
    private List<SkillListElementController> cache = new();

    [SerializeField] private UnityEvent onClickElementButton;
    public event Action<string> OnPanelElementSelected; 

    private SkillListElementController selectedTarget;

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
            IReadOnlyDictionary<string, Skill> mySkillDic = skillManager.GetCurrentSkill();
            int index = 0;

            foreach (var skillId in mySkillDic.Keys)
            {
                //캐시리스트의 인자값(index)에 해당 프리팹이 존재한다면
                //해당 프리팹을 사용해줍니다.
                if (cache.Count > index)
                {
                    cache[index].gameObject.SetActive(true);
                    cache[index].Setting(skillId);
                }
                else
                {
                    var element = Instantiate(elementPreb, prebParent);
                    element.Initial(skillManager);
                    element.OnClickPanelButton += OnClickElementPanelButton; //패널 선택 이벤트 추가
                    element.Setting(skillId);
                    cache.Add(element);
                }

                index++;
            }
        }
    }

    public void ClosePanel()
    {
        foreach (var element in cache)
        {
            element.ResetPanel();
            element.gameObject.SetActive(false);
        }

        selectedTarget = null;
    }

    //이벤트 처리
    private void OnClickElementPanelButton(SkillListElementController _target)
    {
        if (selectedTarget != null)
            selectedTarget.Select(false);

        selectedTarget = _target;
        OnPanelElementSelected?.Invoke(_target.TargetSkillId);

        onClickElementButton?.Invoke();
    }

    private void OnDestroy()
    {
        if(cache != null)
        { 
            foreach (var element in cache)
            { 
                if(element != null)
                    element.OnClickPanelButton -= OnClickElementPanelButton;
            }
        }
    }
}
