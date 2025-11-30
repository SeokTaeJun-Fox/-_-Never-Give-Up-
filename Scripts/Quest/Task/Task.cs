using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);   //상태 변경시 실행하는 이벤트대리자
    public delegate void SuccessChangedHandler(Task task, int currentSuccess, int prevSuccess); //성공횟수 변경시 실행하는 이벤트대리자
    #endregion

    [SerializeField] private Category category; //카테고리 (테스크의 서술어) (ex : 처치하기, 강화하기등)

    [Header("Text")]
    [SerializeField] private string codeName;   //코드네임
    [SerializeField] private string description;    //테스크 설명

    [Header("Action")]
    [SerializeField] private TaskAction action; //테스크 카운터 방식

    [Header("Target")]
    [SerializeField] private TaskTarget[] targets;  //타겟 (테스크의 목적어) (ex : 슬라임,보스등)

    [Header("Setting")]
    [SerializeField] private InitialSuccessValue initialSuccessValue;   //초기 진행 설정
    [SerializeField] private int needSuccessToComplete; //목표 횟수 설정
    [SerializeField] private bool canReceiveReportsDuringCompletion;    //테스크를 완료했어도 계속 성공횟수를 보고 받을 것인지 확인합니다.

    public TaskState state;    //현재 테스크 상태
    private int currentSuccess;     //현재 성공 횟수

    public event StateChangedHandler onStateChanged;  //테스크 상태 변화이벤트
    public event SuccessChangedHandler onSuccessChanged;    //테스크 성공횟수 변할때 발생하는 이벤트

    public int CurrentSuccess   //현재 성공횟수
    { 
        get => currentSuccess;
        set //현재 성공횟수 설정
        {
            int prevSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, needSuccessToComplete);
            if (currentSuccess != prevSuccess)  //현재 성공횟수가 변하면
            {
                State = currentSuccess == needSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }
    public Category Category => category;

    public string CodeName => codeName;

    public string Description => description;

    public int NeedSuccessToComplete => needSuccessToComplete;

    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            Debug.Log($"{description} : {state}");
            onStateChanged?.Invoke(this, state, prevState);
        }
    }
    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        State = TaskState.Running;
        if (initialSuccessValue)
            CurrentSuccess = initialSuccessValue.GetValue(this);
    }

    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }

    public void ReceieveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);
    }

    public void Complete()
    {
        currentSuccess = needSuccessToComplete;
    }

    //이 테스크가 성공 횟수를 보고 받을 대상인지 확인합니다.
    //카테고리의 고드네임이 일치하거나 디스플레이 네임이 일치할경우
    //그리고 타겟이 일치할경우
    //그리고 이 테스트크가 완료상태가아니거나 완료상태여도 완료시 보고받을수있는 테스크일 경우
    //true가 됩니다.
    public bool IsTarget(string category, object target)
        => Category == category &&
        targets.Any(x => x.IsEqual(target)) &&
        (!IsComplete || (IsComplete && canReceiveReportsDuringCompletion));

    //이 테스크가 해당 타겟이 들어있는지 확인합니다.
    public bool ContainsTarget(object target) => targets.Any(x => x.IsEqual(target));
}
