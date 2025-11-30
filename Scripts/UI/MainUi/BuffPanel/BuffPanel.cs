using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//버프 UI 패널을 관리하는 클래스입니다.
//버프 획득 시 UI 요소를 생성하고, 버프 상태에 따라 아이콘과 진행률을 갱신합니다.
public class BuffPanel : MainUIElement
{
    [SerializeField] private BuffElementPanelView preb; //버프 UI 요소 프리팹
    [SerializeField] private Transform viewParent;  //버프 UI 요소 프리팹 부모 트랜스폼
    private Dictionary<ActiveBuff, BuffElementPanelView> viewDic = new(); //버프별 UI 매핑

    private List<BuffElementPanelView> cache = new(); //재사용 가능한 버프 UI 요소 캐시

    //이 클래스에 필요한 인터페이스들
    private IBuffController buffController;

    //외부 의존성 주입 처리 (버프 컨트롤러 연결)
    public override void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IBuffController bc)
                buffController = bc;
        }
    }

    //버프 패널 초기화. (현재 활성화된 버프들을 UI로 표시하고 이벤트 연결.)
    public override void Initial()
    {
        if (buffController == null)
            return;

        var activeBuffs = buffController.ActiveBuffs;
        foreach (var activeBuff in activeBuffs)
        {
            if(!activeBuff.IsEnd)
                CreateView(activeBuff);
        }

        buffController.OnGetBuff += CreateView;
    }

    //버프 UI 요소를 생성하거나 캐시에서 재활용하여 표시합니다.
    private void CreateView(ActiveBuff _activeBuff)
    {
        //캐시에서 안쓰는 view를 불러옵니다. 없으면 생성합니다.
        var view = cache.FirstOrDefault(x => !x.gameObject.activeSelf);
        if (view != null)
        {
            view.gameObject.SetActive(true);
            view.transform.SetAsLastSibling();
        }
        else
        {
            view = Instantiate(preb, viewParent);
            cache.Add(view);
        }

        //view세팅합니다.
        float rate = 1 - _activeBuff.RemainTimeRate;
        Sprite buffIcon = _activeBuff.Buff.Icon;
        view.Setting(buffIcon, rate);

        //activeBuff를 딕셔너리에 저장합니다.
        if (!viewDic.ContainsKey(_activeBuff))
        {
            viewDic.Add(_activeBuff, view);
        }
        else
        {
            viewDic[_activeBuff] = view;
        }

        //activeBuff 이벤트 세팅
        _activeBuff.OnUpdate += OnUpdate;
        _activeBuff.OnEnd += OnEnd;
    }

    //버프 종료 시 UI 요소를 비활성화하고 딕셔너리(현재활성버프 딕셔너리)에서 제거합니다.
    private void OnEnd(ActiveBuff _activeBuff)
    {
        if (viewDic.ContainsKey(_activeBuff))
        {
            viewDic[_activeBuff].Disable();
            viewDic.Remove(_activeBuff);
        }
        else
        {
            Debug.LogWarning($"버프 패널에 {_activeBuff.Buff.BuffName}이벤트가 추가되지않았습니다.");
        }
    }

    //버프 갱신 시 UI 요소의 진행률을 업데이트합니다.
    private void OnUpdate(ActiveBuff _activeBuff)
    {
        if (viewDic.ContainsKey(_activeBuff))
        {
            float rate = 1 - _activeBuff.RemainTimeRate;
            Sprite buffIcon = _activeBuff.Buff.Icon;
            viewDic[_activeBuff].Setting(buffIcon, rate);
        }
        else
        {
            Debug.LogWarning($"버프 패널에 {_activeBuff.Buff.BuffName}이벤트가 추가되지않았습니다.");
        }
    }

    //오브젝트 파괴 시 이벤트 연결 해제합니다.
    private void OnDestroy()
    {
        if (buffController != null)
            buffController.OnGetBuff -= CreateView;
    }
}
