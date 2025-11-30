using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAbilityController : MonoBehaviour
{
    [SerializeField] private MonsterAbility initialMonsterAbility;  //초기 몬스터 능력치
    [SerializeField] private MonsterAbility curMonsterAbility;   //현재 몬스터 능력치/상태 (디버그용 시리얼라이즈)

    //몬스터 능력치를 읽기전용 딕셔너리로 불러옵니다
    public IReadOnlyDictionary<PlayerStat, object> MonsterAbilities => curMonsterAbility.CharacterAbilities;
    public event Action<IReadOnlyDictionary<PlayerStat, object>> OnAbilityChanged;

    public void IncreaseHp(int _amount) { curMonsterAbility.Hp += _amount; }
    public void IncreaseMp(int _amount) { curMonsterAbility.Mp += _amount; }

    public void ChangeMaxHp(int _maxHp) { curMonsterAbility.MaxHp = _maxHp; }
    public void ChangeMaxMp(int _maxMp) { curMonsterAbility.MaxMp = _maxMp; }

    public void IncreaseAddMaxHp(int _amount) { curMonsterAbility.AddMaxHp += _amount; }
    public void IncreaseAddMaxMp(int _amount) { curMonsterAbility.AddMaxMp += _amount; }

    public void ChangeAtk(int _atk) { curMonsterAbility.Atk = _atk; }
    public void IncreaseAddAtk(int _amount) { curMonsterAbility.AddAtk += _amount; }

    public void ChangeDef(int _def) { curMonsterAbility.Def = _def; }
    public void IncreaseAddDef(int _amount) { curMonsterAbility.AddDef += _amount; }

    public void ChangeLevel(int _level) { curMonsterAbility.Level = _level; }

    private void Awake()
    {
        curMonsterAbility = Instantiate(initialMonsterAbility);
        curMonsterAbility.Initial();
        curMonsterAbility.OnAbilityChanged += OnAbilityChanged;
    }

    private void OnDestroy()
    {
        curMonsterAbility.OnAbilityChanged -= OnAbilityChanged;
    }

    public void Initial(MonsterAbility _monsterAbility)
    {
        curMonsterAbility.CopyAbility(_monsterAbility);
    }

    //이벤트
    public void OnAbilityChangedMethod(IReadOnlyDictionary<PlayerStat, object> abilities)
    {
        OnAbilityChanged?.Invoke(abilities);
    }
}
