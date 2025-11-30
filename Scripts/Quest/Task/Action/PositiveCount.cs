using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/PositiveCount", fileName = "Positive Count")]
public class PositiveCount : TaskAction //현재 들어온 성공횟수가 양수일경우 카운트됩니다.
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : currentSuccess; 
    }
}
