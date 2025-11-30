using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum QuestState
{ 
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion    //유저가 Quest 완료를 해주길 기다리는 상태입니다.
                            //(퀘스트는 달성했지만 완료상태가 아닌 상태입니다.)

}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Event
    //보고 받았을때 실행할 이벤트 대리자
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    //퀘스트를 완료했을때 실행할 이벤트 대리자
    public delegate void CompletedHandler(Quest quest);
    //퀘스트 완료 준비시 실행할 이벤트 대리자
    public delegate void CompleteReadyHandler(Quest quest);
    //퀘스트를 취소했을때 실행할 이벤트 대리자
    public delegate void CanceledHandler(Quest quest);
    //다음 테스크 그룹이 시작했을대 실행할 이벤트 대리자
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion

    [SerializeField] private Category category;
    [SerializeField] private Sprite icon;

    [Header("Text")]
    [SerializeField] private string codeName;
    [SerializeField] private string displayName;
    [SerializeField, TextArea] private string description;

    [Header("Task")]
    [SerializeField] private TaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField] private Reward[] rewards;

    [Header("Option")]
    [SerializeField] private bool useAutoComplete;
    [SerializeField] private bool isCancelable;
    [SerializeField] private bool isSavable;

    [Header("Condition")]
    [SerializeField] Condition[] acceptionConditions;
    [SerializeField] Condition[] cancelConditions;

    [Header("ActionObject")]
    [SerializeField] private ActionObject[] completeActions;

    private int currentTaskGroupIndex; //현재 가리키고있는 테스크 그룹 배열의 인덱스
                                       //기능구현 : 테스크 그룹을 순차적으로 진행

    private bool isInvokeWaitCompletion;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex]; //현재 테스크 그룹
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;
    public bool IsRegistered => State != QuestState.Inactive;   //퀘스트 목록에 있는지 확인한다
    public bool isComplatable => State == QuestState.WaitingForCompletion;  //퀘스트가 완료대기인지 확인한다.
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;

    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    //특정 조건을 만족하지 못하면 취소할수 없습니다.
    //ex : 레벨 100렙달성 전까지는 퀘스트를 취소할 수 없는 기능을 구현할수 있습니다.
    public virtual bool IsSavable => isSavable;

    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));

    //하나의 Quest가 여러 개의 Task 묶음을 가질 수 있습니다.

    public event TaskSuccessChangedHandler onTaskSuccessChanged; //보고 받았을때 실행할 이벤트변수
    public event CompletedHandler onCompleted; //퀘스트를 완료했을때 실행할 이벤트변수
    public event CompleteReadyHandler onCompleteReady;
    public event CanceledHandler onCanceled; //퀘스트를 취소했을때 실행할 이벤트변수
    public event NewTaskGroupHandler onNewTaskGroup; //다음 테스크 그룹이 시작했을대 실행할 이벤트변수

    //퀘스트(Quest)가 시스템에 등록되었을 때 실행됩니다.
    public void OnRegister()
    {
        //이미 등록된 퀘스트를 또 등록할 경우 오류 메시지 출력합니다.
        Debug.Assert(!IsRegistered, "This quest has already been registered.");

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.onSuccessChanged += OnSuccessChanged;
        }

        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }

    //퀘스트 시스템의 onregister이벤트가 실행 후 이 함수가 실행되도록 설정하였습니다.
    public void LateOnRegister()
    {
        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            State = QuestState.WaitingForCompletion;
            if (useAutoComplete)
                Complete();
            else if (!isInvokeWaitCompletion)
            {
                onCompleteReady?.Invoke(this);
                isInvokeWaitCompletion = true;
            }
        }
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            //다음 테스크 그룹이 없다면
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                State = QuestState.WaitingForCompletion;
                if (useAutoComplete)
                    Complete();
                else if (!isInvokeWaitCompletion)
                {
                    onCompleteReady?.Invoke(this);
                    isInvokeWaitCompletion = true;
                }
            }
            else
            {
                var prevTaskGroup = taskGroups[currentTaskGroupIndex++];
                prevTaskGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
        {
            State = QuestState.Running;
            isInvokeWaitCompletion = false;
        }
    }

    public void Complete()  //용도 : 퀘스트를 무조건 완료하는 아이템이나, 세이브시스템에 의해서 사용되는 경우입니다.
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        foreach (var action in completeActions)
            action.Action();

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleteReady = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This quest can't be canceled");

        State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    public bool ContainsTarget(object target) => taskGroups.Any(x => x.ContainsTarget(target));

    public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);

    //퀘스트 인스턴스 복제
    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            codeName = codeName,
            state = State,
            taskgroupIndex = currentTaskGroupIndex,
            taskSuccessCounts = CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray()
        };
    }

    public void LoadFrom(QuestSaveData saveData)
    {
        State = saveData.state;
        currentTaskGroupIndex = saveData.taskgroupIndex;

        //이전의 테스크그룹들을 완료처리합니다.
        for (int i = 0; i < currentTaskGroupIndex; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        CurrentTaskGroup.Start();

        for (int i = 0; i < saveData.taskSuccessCounts.Length; i++)
        {
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCounts[i];
        }
    }

    private void OnSuccessChanged(Task task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    [Conditional("UNITY_EDITOR")]   
    //인자로 전달한 Simbol값이 선언되어 있으면 함수를 실행합니다.
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been canceled.");
        Debug.Assert(!IsComplete, "This quest has already been completed.");
    }
}
