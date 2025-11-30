using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 능력치 및 경험치 시스템을 정의하는 인터페이스입니다.
/// UI 및 게임 시스템등과의 연결을 위한 이벤트와 기능을 제공합니다.
/// </summary>
public interface IPlayerAbilityManager
{
    //플레이어 능력치를 읽기전용 딕셔너리로 불러옵니다
    public IReadOnlyDictionary<PlayerStat, object> PlayerAbilities { get; }

    //능력치 변경 이벤트
    public event Action<IReadOnlyDictionary<PlayerStat, object>> OnAbilityChanged;
    public event Action<int> OnLevelUp; //레벨업 이벤트
    public event Action<int, int> OnExpChanged; //경험치 변경 이벤트

    public int GetCurrentExp { get; }
    public int MaxExp { get; }

    //능력치 변경 프로퍼티
    public void IncreaseHp(int _amount);
    public void IncreaseMp(int _amount);

    public void SetHp(int _amount);
    public void SetMp(int _amount);

    public void ChangeMaxHp(int _maxHp);
    public void ChangeMaxMp(int _maxMp);

    public void IncreaseAddMaxHp(int _amount);
    public void IncreaseAddMaxMp(int _amount);

    public void ChangeAtk(int _atk);
    public void IncreaseAddAtk(int _amount);

    public void ChangeDef(int _def);
    public void IncreaseAddDef(int _amount);

    public void ChangeAttackRange(float _attackRange);

    public void ChangeLevel(int _level);

    //이벤트 전달
    public void OnAbilityChangedMethod(IReadOnlyDictionary<PlayerStat, object> abilities);

    public void SetPlayerAbility(int _playerLevel);

    public void AddExp(int _exp);

    void Initial();
}
