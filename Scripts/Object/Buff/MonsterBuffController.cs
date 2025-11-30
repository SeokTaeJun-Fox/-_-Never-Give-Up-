using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterBuffController : MonoBehaviour, IBuffController
{
    [SerializeField] private MonsterController ownerController;
    [SerializeField] private MonsterInfoUIClient uiClient;
    public IReadOnlyList<ActiveBuff> ActiveBuffs => activeBuffs;
    public event Action<ActiveBuff> OnGetBuff;

    private List<ActiveBuff> activeBuffs = new();
    private IBuffUser owner;
    private bool isStop;

    private void Awake()
    {
        SetOwner(ownerController);
        ownerController.SetBuffController(this);
        uiClient.SetBuffController(this);
    }

    private void Update()
    {
        if (owner == null || isStop)
            return;

        for (int index = activeBuffs.Count - 1; index >= 0; index--)
        {
            if (!activeBuffs[index].IsEnd)
                activeBuffs[index].UpdateTime(Time.deltaTime);
            else
                activeBuffs.Remove(activeBuffs[index]);
        }
    }

    public void SetOwner(IBuffUser _owner)
    {
        owner = _owner;
    }

    public void GetBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility, ICharacterSoundController _sfxSoundController)
    {
        if (_sfxSoundController != null && _buff.Sfx != string.Empty)
            _sfxSoundController.PlayOneShotSFX(_buff.Sfx);

        if (IsExistActiveBuff(_buff))
        {
            ActiveBuff replayActiveBuff = activeBuffs.FirstOrDefault(x => x.Code == _buff.BuffCode);
            replayActiveBuff?.Replay();
            return;
        }

        ActiveBuff activeBuff = new ActiveBuff(_buff, owner, _providerAbility);
        activeBuffs.Add(activeBuff);
        activeBuff.Start();
        //activeBuff.OnEnd += RemoveActiveBuffInList;

        OnGetBuff?.Invoke(activeBuff);
    }

    public void ClearAllBuff()
    {
        for (int index = activeBuffs.Count - 1; index >= 0; index--)
        {
            activeBuffs[index].End();
        }
    }

    public void ClearBuff(Buff _buff)
    {
        ActiveBuff clearBuff = activeBuffs.FirstOrDefault(x => x.Code == _buff.BuffCode);

        if (clearBuff != null)
        {
            clearBuff.End();
        }
    }

    public void ClearBuff(List<Category> _buffCategoryList)
    {
        var clearActiveBuffList = activeBuffs.Where(x => IsCorrespondToCategory(_buffCategoryList, x));
        foreach (var clearActiveBuff in clearActiveBuffList)
        {
            clearActiveBuff.End();
        }
    }

    public void Play()
    {
        isStop = false;
        foreach (var activeBuff in activeBuffs)
        {
            activeBuff.SetOwner(owner);
            activeBuff.Start();
        }
    }

    public void Stop()
    {
        isStop = true;
        foreach (var activeBuff in activeBuffs)
            activeBuff.Pause();
    }

    private void RemoveActiveBuffInList(ActiveBuff _activeBuff)
        => activeBuffs.Remove(_activeBuff);

    private bool IsExistActiveBuff(Buff _buff)
        => activeBuffs.Any(x => x.Code == _buff.BuffCode);

    //bool
    //버프의 가테고리가 카테고리 리스트(_buffCategoryList)중 한개이상이 해당되면 true로 반환됩니다.
    private bool IsCorrespondToCategory(List<Category> _buffCategoryList, ActiveBuff _activeBuff)
    {
        return _buffCategoryList.Any(x => x.CodeName == _activeBuff.Buff.Category.CodeName);
    }
}
