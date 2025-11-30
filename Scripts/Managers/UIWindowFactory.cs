using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UIRegistry에 등록된 UIEntry 정보를 기반으로 UI 윈도우를 생성하고 초기화하는 팩토리 클래스입니다.
/// 의존성 주입, 캔버스 배치, 중복 생성 방지 등의 기능을 제공합니다.
/// </summary>
public class UIWindowFactory : MonoBehaviour
{
    [SerializeField] private UIRegistry uiRegistry; //UIEntry 등록소
    [SerializeField] private GetCanvasRequestEvent getCanvasRequestEvent;   //캔버스 요청 이벤트
    [SerializeField] private ShowUIWindowFactoryEvent showUIWindowFactoryEvent; //윈도우 생성 요청 이벤트

    private Dictionary<UIType, UIEntry> entryMap;   //UIType별 UIEntry 매핑
                                                    //윈도우 생성시 Entry에 등록되어있는지 검사하는데 사용됩니다.
    private Dictionary<UIType, GameObject> existWindowInScene = new();  //씬에 존재하는 윈도우 오브젝트 캐시

    private void Awake()
    {
        showUIWindowFactoryEvent.OnShowWindow += ShowWindow;
    }

    private void Start()
    {
        //UIEntry 리스트를 딕셔너리로 변환
        entryMap = uiRegistry.Entries.ToDictionary(e => e.UiType, e => e);
    }

    /// <summary>
    /// UI 윈도우를 생성하거나 이미 존재하는 경우 재활용하여 보여줍니다.
    /// </summary>
    /// <param name="_uiType"></param>
    /// <param name="_initialDatas"></param>
    public void ShowWindow(UIType _uiType, object[] _initialDatas = null)
    {
        if (!entryMap.TryGetValue(_uiType, out var entry))
        {
            Debug.LogWarning($"entryMap딕셔너리에 {_uiType}타입을 등록하지 않았습니다.");
            return;
        }

        //윈도우가 아직 생성되지 않은 경우
        if (!existWindowInScene.ContainsKey(_uiType))
        {
            //엔트리정보 사용
            var prefab = entry.Prefab;
            if (prefab == null)
            {
                Debug.LogError($"{_uiType}타입의 UI prefab을 등록하지 않았습니다.");
                return;
            }

            //의존성 주입될 객체를 담슴니다.
            object[] dependencies = ResolveDependencies(entry);

            //부모 캔버스 설정
            GameObject parent = null;
            if (getCanvasRequestEvent != null)
            {
                parent = getCanvasRequestEvent.RaiseGet(entry.CanvasType);
            }
            var instance = Instantiate(prefab, parent.transform);

            //윈도우 초기화 및 열기
            var ui = instance.GetComponent<IUIWindow>();
            ui?.InjectDependencies(dependencies);
            ui?.Initial(_initialDatas);
            ui?.Open();

            //생성된 윈도우 캐싱
            existWindowInScene.Add(_uiType, instance);
        }
        else
        {
            //이미 존재하는 윈도우 재활용
            IUIWindow ui = existWindowInScene[_uiType].GetComponent<IUIWindow>();
            if (!existWindowInScene[_uiType].activeInHierarchy)
            {
                ui?.Initial(_initialDatas);
                ui?.Open();
            }
            else
            {
                ui?.Initial(_initialDatas);
            }
        }
    }

    /// <summary>
    /// UIEntry에 정의된 의존성 인터페이스 이름을 기반으로 서비스 로케이터에서 객체를 가져옵니다.
    /// </summary>
    /// <param name="_entry"></param>
    /// <returns></returns>
    private object[] ResolveDependencies(UIEntry _entry)
    {
        List<object> resolved = new List<object>();

        foreach (var typeName in _entry.DependencyInterfaceTypeNames)
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

    /// <summary>
    /// 문자열로 받은 인터페이스 이름을 실제 타입으로 변환합니다.
    /// </summary>
    /// <param name="_typeName"></param>
    /// <returns></returns>
    private Type GetInterfaceType(string _typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsInterface && t.Name == _typeName);
    }

    private void OnDestroy()
    {
        showUIWindowFactoryEvent.OnShowWindow -= ShowWindow;
    }
}
