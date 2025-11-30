using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroupState
{ 
    Inactive,
    Running,
    Complete
}

[System.Serializable]
public class TaskGroup
{
    [SerializeField] private Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;
    public Quest Owner { get; private set; }
    public bool IsAllTaskComplete => tasks.All(x => x.IsComplete);     //테스크 그룹안에 모든 테스크가 완료상태인지
                                                                       //확인합니다.
    public bool IsComplete => State == TaskGroupState.Complete;
    public TaskGroupState State { get; private set; }

    //테스크 그룹 복제
    public TaskGroup(TaskGroup copyTarget)
    {
        tasks = copyTarget.Tasks.Select(x => Object.Instantiate(x)).ToArray();
    }

    public void Setup(Quest owner)
    {
        Owner = owner;
        foreach (var task in tasks)
            task.Setup(owner);
    }

    public void Start()
    {
        State = TaskGroupState.Running;
        foreach (var task in tasks)
            task.Start();
    }

    public void End()
    {
        State = TaskGroupState.Complete;
        foreach (var task in tasks)
            task.End();
    }

    //테스크 그룹에게 성공횟수를 보고를 받는 함수입니다.
    public void ReceiveReport(string category, object target, int successCount)
    {
        foreach (var task in tasks)
        {
            if (task.IsTarget(category, target))
                task.ReceieveReport(successCount);
        }
    }

    //테스크 그룹을 완료상태로 만듭니다.
    public void Complete()
    {
        if (IsComplete)
            return;

        State = TaskGroupState.Complete;

        //테스크 그룹내 모든 테스크를 완료상태로 만듭니다.
        foreach (var task in tasks)
        { 
            if (!task.IsComplete)
                task.Complete();
        }
    }

    public Task FindTaskByTarget(object target) => tasks.FirstOrDefault(x => x.ContainsTarget(target));

    //해당 타겟이 있는 테스크를 가져옵니다.
    public Task FindTaskByTarget(TaskTarget target) => FindTaskByTarget(target.Value);

    //해당 타겟이 들어있는 테스크가 있는지 확인합니다.
    public bool ContainsTarget(object target) => tasks.Any(x => x.ContainsTarget(target));

    public bool ContainsTarget(TaskTarget target) => ContainsTarget(target.Value);
}
