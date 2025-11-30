using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//플레이어에게 적용된 버프를 관리하는 컨트롤러입니다.
//버프의 실행, 갱신, 종료, 일시정지, 재시작 등을 처리합니다.
public class PlayerBuffController : MonoBehaviour, IBuffController
{
    public IReadOnlyList<ActiveBuff> ActiveBuffs => activeBuffs;
    public event Action<ActiveBuff> OnGetBuff;  //버프 부여시 이벤트가 발생됩니다.

    private List<ActiveBuff> activeBuffs = new();
    private IBuffUser owner;
    private bool isStop;

    private void Awake()
    {
        ServiceLocator.Register<IBuffController>(this);
    }

    private void Update()
    {
        //버프 갱신 및 종료 처리
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

    //버프 적용 대상을 설정합니다.
    public void SetOwner(IBuffUser _owner)
    {
        owner = _owner;
    }

    //버프를 적용합니다. 중복 시 재식작 처리하며, 사운드도 재생합니다.
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

        OnGetBuff?.Invoke(activeBuff);
    }

    //모든 버프를 종료(삭제)합니다.
    public void ClearAllBuff()
    {
        for (int index = activeBuffs.Count - 1; index >= 0; index--)
        {
            activeBuffs[index].End();
        }
    }

    //특정 버프를 종료(삭제)합니다.
    public void ClearBuff(Buff _buff)
    {
        ActiveBuff clearBuff = activeBuffs.FirstOrDefault(x => x.Code == _buff.BuffCode);

        if (clearBuff != null)
        {
            clearBuff.End();
        }
    }

    //특정 카테고리에 해당하는 버프들을 종료(삭제)합니다.
    public void ClearBuff(List<Category> _buffCategoryList)
    {
        var clearActiveBuffList = activeBuffs.Where(x => IsCorrespondToCategory(_buffCategoryList, x));
        foreach (var clearActiveBuff in clearActiveBuffList)
        { 
            clearActiveBuff.End();
        }
    }

    //버프 시스템을 재시작합니다.
    public void Play()
    {
        isStop = false;
        foreach (var activeBuff in activeBuffs)
        {
            activeBuff.SetOwner(owner);
            activeBuff.Start();
        }
    }

    //버프 시스템을 일시정지합니다.
    public void Stop()
    {
        isStop = true;
        foreach(var activeBuff in activeBuffs)
            activeBuff.Pause();
    }

    //버프 리스트에서 특정 버프를 제거합니다.
    private void RemoveActiveBuffInList(ActiveBuff _activeBuff)
        => activeBuffs.Remove(_activeBuff);

    //해당 버프가 이미 적용되어 있는지 확인합니다.
    private bool IsExistActiveBuff(Buff _buff)
        => activeBuffs.Any(x => x.Code == _buff.BuffCode);

    //버프가 특정 카테고리에 속하는지 확인합니다.
    //버프의 가테고리가 카테고리 리스트(_buffCategoryList)중 한개이상이 해당되면 true로 반환됩니다.
    private bool IsCorrespondToCategory(List<Category> _buffCategoryList, ActiveBuff _activeBuff)
    {
        return _buffCategoryList.Any(x => x.CodeName == _activeBuff.Buff.Category.CodeName);
    }
}
