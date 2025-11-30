using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//퀵 퀘스트 패널의 요소내 퀘스트 설명텍스트를 보여주는 클래스입니다.
public class QuickQuestElementDesc : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descTmp;
    [SerializeField] private Color notWaitCompleteColor;
    [SerializeField] private Color waitCompletedColor;

    [SerializeField] private Task deTesk;

    public void SetDesc(Task _task)
    {
        deTesk = _task;
        descTmp.color = _task.IsComplete ? waitCompletedColor : notWaitCompleteColor;
        descTmp.text = $"▶ {_task.Description} ({_task.CurrentSuccess}/{_task.NeedSuccessToComplete})";
    }
}
