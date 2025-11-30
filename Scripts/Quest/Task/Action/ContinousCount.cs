using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/ContinousCount", fileName = "Continous Count")]
public class ContinousCount : TaskAction    //현재 들어온 성공값이 양수일경우 카운트,  음수일경우 초기화가됩니다.
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount > 0 ? currentSuccess + successCount : 0;
    }
}
