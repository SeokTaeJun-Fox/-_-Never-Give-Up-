using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerController의 부재로인해 장비 착용시 능력치가 안올라가므로
//PlayerController가 존재하지않으면 이 클래스가 대신 기능을 수행합니다.
public class ItemUserDeputy : MonoBehaviour, IItemUser
{
    public IReadOnlyDictionary<PlayerStat, object> PlayerAbilities => playerAbilityManager?.PlayerAbilities;

    private IPlayerAbilityManager playerAbilityManager;

    public void SetAbilityManager(IPlayerAbilityManager _playerAbilityManager)
    { 
        playerAbilityManager = _playerAbilityManager;
    }

    public void IncreaseHp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseHp(_amount);
    }

    public void IncreaseMp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseMp(_amount);
    }

    public void IncreaseAddMaxHp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddMaxHp(_amount);
    }

    public void IncreaseAddMaxMp(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddMaxMp(_amount);
    }

    public void IncreaseAddAtk(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddAtk(_amount);
    }

    public void IncreaseAddDef(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.IncreaseAddDef(_amount);
    }

    public void ChangeAttackRange(int _amount)
    {
        if (playerAbilityManager != null)
            playerAbilityManager.ChangeAttackRange(_amount);
    }

    public void TakeBuff(Buff _buff, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {

    }

    public void ClearBuff(List<Category> _categories)
    {

    }
}
