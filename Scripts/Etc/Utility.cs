using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utility
{
    /// <summary>
    /// 빈 곳을 터치했는지 확인합니다.
    /// </summary>
    /// <param name="touchPos">확인할 터치 위치</param>
    /// <returns>빈 곳이면 false</returns>
    public static bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    //a,b사이의 각도를 구합니다.
    public static float GetDegree(Vector3 a, Vector3 b)
    {
        // a벡터와 b벡터를 내적합니다.
        float dot = Vector3.Dot(a.normalized, b.normalized);
        // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구합니다.
        float theta = Mathf.Acos(dot);
        // 세타를 각도로 변환합니다.
        float degree = Mathf.Rad2Deg * theta;

        return degree;
    }

    //애니메이터에 모든 트리거 파라미터를 초기화합니다.
    public static void ResetAllTrigger(Animator _animator)
    {
        AnimatorControllerParameter[] triggerParams = _animator.parameters.Where(acp => acp.type == AnimatorControllerParameterType.Trigger).ToArray();
        foreach (AnimatorControllerParameter triggerParam in triggerParams)
        {
            _animator.ResetTrigger(triggerParam.name);
        }
    }

    public static object[] ResolveDependencies(string[] _dependencyInterfaceTypeNames)
    {
        List<object> resolved = new List<object>();

        foreach (var typeName in _dependencyInterfaceTypeNames)
        {
            var type = GetInterfaceType(typeName);
            if (type == null)
            {
                Debug.LogError($"{typeName}타입형의 클래스는 존재하지 않습니다.");
                continue;
            }

            var service = ServiceLocator.Get(type);
            if (service == null)
            {
                Debug.LogError($"서비스로케이터에 {typeName}타입을 등록하지 않았습니다.");
            }

            resolved.Add(service);
        }

        return resolved.ToArray();
    }

    public static Type GetInterfaceType(string _typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsInterface && t.Name == _typeName);
    }
}
