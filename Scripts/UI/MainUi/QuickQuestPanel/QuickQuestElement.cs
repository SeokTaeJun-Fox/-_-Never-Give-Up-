using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class QuickQuestElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private QuickQuestElementDesc preb;
    [SerializeField] private Transform prebParent;
    [SerializeField] private List<QuestCategoryColor> questCategoryColors;

    private Quest target;
    private Dictionary<Task, QuickQuestElementDesc> descUiDic = new(); 

    public void Setting(Quest _quest)
    { 
        target = _quest;

        //타이틀 텍스트 색 지정
        if (questCategoryColors.Exists(x => x.category == _quest.Category))
            titleTmp.color = questCategoryColors.FirstOrDefault(x => x.category == _quest.Category).color;
        else
            titleTmp.color = Color.white;

        //타이틀 텍스트 값 설정
        string frontText = (_quest.Category == null) ? "" : "[" + _quest.Category.DisplayName + "] "; 
        titleTmp.text = $"{frontText}{_quest.DisplayName}";

        //설명 텍스트 설정
        if (_quest.TaskGroups.Count != 0)
        {
            foreach (var task in _quest.CurrentTaskGroup.Tasks)
            {
                var newObject = Instantiate(preb, prebParent);
                newObject.SetDesc(task);
                descUiDic.Add(task, newObject);

                task.onSuccessChanged += OnSuccessChanged;
            }
        }

        //퀘스트 이벤트설정
        _quest.onCompleted += OnCompleted;
        _quest.onCanceled += OnCanceled;
    }

    private void OnCompleted(Quest _quest)
    {
        Destroy(gameObject);
    }

    private void OnCanceled(Quest _quest)
    {
        Destroy(gameObject);
    }

    private void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
    {
        descUiDic[task].SetDesc(task);
    }


    private void OnDestroy()
    {
        if (target == null)
            return;

        target.onCompleted -= OnCompleted;
        target.onCanceled -= OnCanceled;

        if (target.TaskGroups.Count != 0)
        {
            foreach (var task in target.CurrentTaskGroup.Tasks)
            {
                task.onSuccessChanged -= OnSuccessChanged;
            }
        }
    }
}

[Serializable]
public class QuestCategoryColor
{
    public Category category;
    public Color color;
}
