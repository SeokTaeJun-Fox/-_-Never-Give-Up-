using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterBuffPanel : MonoBehaviour
{
    [SerializeField] private BuffElementPanelView preb;
    [SerializeField] private Transform viewParent;
    private Dictionary<ActiveBuff, BuffElementPanelView> viewDic = new();

    private List<BuffElementPanelView> cache = new();

    //이 클래스에 필요한 인터페이스들
    private IBuffController buffController;

    public void SetBuffController(IBuffController _buffController)
    { 
        buffController = _buffController;
    }

    public void ActivePanel()
    {
        if (buffController == null)
            return;

        var activeBuffs = buffController.ActiveBuffs;
        foreach (var activeBuff in activeBuffs)
        {
            if (!activeBuff.IsEnd)
                CreateView(activeBuff);
        }

        buffController.OnGetBuff += CreateView;
    }

    //이벤트
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

        //view를 딕셔너리에 저장합니다.
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

    private void OnEnd(ActiveBuff _activeBuff)
    {
        if (viewDic.ContainsKey(_activeBuff))
        {
            viewDic[_activeBuff].Disable();
            viewDic.Remove(_activeBuff);
        }
        //else
        //{
        //    Debug.LogWarning($"버프 패널에 {_activeBuff.Buff.BuffName}이벤트가 추가되지않았습니다.");
        //}
    }

    private void OnUpdate(ActiveBuff _activeBuff)
    {
        if (viewDic.ContainsKey(_activeBuff))
        {
            float rate = 1 - _activeBuff.RemainTimeRate;
            Sprite buffIcon = _activeBuff.Buff.Icon;
            viewDic[_activeBuff].Setting(buffIcon, rate);
        }
        //else
        //{
        //    Debug.LogWarning($"버프 패널에 {_activeBuff.Buff.BuffName}이벤트가 추가되지않았습니다.");
        //}
    }

    public void Initial()
    {
        if (buffController != null)
            buffController.OnGetBuff -= CreateView;

        buffController = null;

        foreach (var view in viewDic.Values)
        {
            view.Disable();
        }
        viewDic.Clear();
    }

    private void OnDestroy()
    {
        if (buffController != null)
            buffController.OnGetBuff -= CreateView;
    }
}
