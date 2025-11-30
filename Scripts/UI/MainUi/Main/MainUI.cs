using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MainUI : MonoBehaviour
{
    [SerializeField] private MainUIElementEntry[] mainUIElementRegister;

    private void Start()
    {
        StartCoroutine(CoInitialAndDIRoutine());
    }

    private IEnumerator CoInitialAndDIRoutine()
    {
        yield return null;

        SettingMainUIElements();
    }

    public void SettingMainUIElements()
    {
        foreach (var mainUIElementSettingInfo in mainUIElementRegister)
        {
            object[] diObjects = ResolveDependencies(mainUIElementSettingInfo.DependencyInterfaceTypeNames);
            mainUIElementSettingInfo.mainUIElement.InjectDependencies(diObjects);
            mainUIElementSettingInfo.mainUIElement.Initial();
        }
    }

    private object[] ResolveDependencies(string[] _dependencyInterfaceTypeNames)
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

    private Type GetInterfaceType(string _typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsInterface && t.Name == _typeName);
    }
}

[Serializable]
public struct MainUIElementEntry
{
    public MainUIElement mainUIElement;
    public string[] DependencyInterfaceTypeNames;
}
