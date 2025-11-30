using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

//실행 중인 버프의 상태와 타이머를 관리하는 클래스입니다.
//시작, Tick, 종료, 일시정지, 재시작 기능을 제공합니다.
public class ActiveBuff
{
    private float remainTime;   //남은 시간
    private float remainTickTime;
    private Buff buff;
    private IBuffUser user;
    private IReadOnlyDictionary<PlayerStat, object> providerAbility;

    public event Action<ActiveBuff> OnUpdate;   //버프 중에 매 프레임마다 발생됩니다.
    public event Action<ActiveBuff> OnEnd;      //버프가 끝나면 이벤트가 발생됩니다. 

    public string Code { get => Buff.BuffCode; }
    public float RemainTime { get => remainTime; }
    public Buff Buff { get => buff; }
    public IBuffUser User { get => user; }
    public IReadOnlyDictionary<PlayerStat, object> ProviderAbility { get => providerAbility; }

    private bool isEnd;
    public bool IsEnd => isEnd;

    public float RemainTimeRate
    {
        get => remainTime / buff.BuffTime;
    }

    public ActiveBuff(Buff _buff, IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        buff = _buff;
        user = _user;
        providerAbility = _providerAbility;
        remainTime = Buff.BuffTime;
        remainTickTime = buff.TickTime;
    }

    //버프를 시작합니다. (효과 적용 및 파티클 실행)
    public void Start() 
    {
        isEnd = false;
        Buff.OnStart(User, ProviderAbility);
    }

    //Tick 주기마다 호출되는 지속 효과 실행
    //(isTick = true일시)
    public void Tick()
    {
        Buff.OnTick(User, ProviderAbility);
    }

    //버프를 종료합니다. (효과 제거 및 파티클 종료)
    public void End()
    {
        isEnd = true;

        Buff.OnEnd(User, ProviderAbility);
        OnEnd?.Invoke(this);
        Initial();
    }

    //버프를 일시정지합니다. (효과 제거)
    public void Pause()
    {
        Buff.OnEnd(User, ProviderAbility);
    }

    //버프의 남은 시간을 갱신하고, Tick 및 종료 여부를 판단합니다.
    public void UpdateTime(float _elapsedTime)
    {
        remainTime -= _elapsedTime;

        if (buff.IsTick)
        {
            remainTickTime -= _elapsedTime;
            if (remainTickTime <= 0)
            {
                Tick();

                if (buff == null)
                    return;

                remainTickTime = buff.TickTime;
            }
        }

        OnUpdate?.Invoke(this);

        if (RemainTime <= 0)
        {
            End();
        }
    }

    //버프를 재시작합니다. (남은 시간 초기화)
    public void Replay()
    {
        remainTime = buff.BuffTime;
    }

    //버프 대상을 설정합니다.
    public void SetOwner(IBuffUser _user)
    { 
        user = _user;
    }

    //데이터를 초기화합니다.
    public void Initial()
    {
        remainTime = 0;
        remainTickTime = 0;
        buff = null;
        user = null;
        providerAbility = null;

        OnUpdate = null;
        OnEnd = null;
    }
}
