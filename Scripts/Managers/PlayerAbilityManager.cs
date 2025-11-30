using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 능력치와 경험치, 레벨업을 관리하는 클래스입니다.
/// CSV 데이터를 기반으로 능력치를 설정하고, 이벤트를 통해 UI, 게임시스템과 상호작용합니다.
/// </summary>
public class PlayerAbilityManager : MonoBehaviour, IPlayerAbilityManager
{
    [SerializeField] private PlayerAbility curPlayerAbility;   //현재 플레이어 능력치 객체
    [SerializeField] private TextAsset playerLevelInfoTextAsset; //레벨별 능력치 정보가 담긴 CSV파일

    private CSVObject playerLevelInfo;  // CSV 파싱 오브젝트

    private int currentExp; //현재 경험치
    private int maxExp; //현재 레벨부터 다음레벨까지 필요한 경험치량
    private int maxLv = 5;  //최대 레벨

    public int GetCurrentExp => currentExp;
    public int MaxExp => maxExp;

    //능력치 딕셔너리 및 관련 이벤트
    public IReadOnlyDictionary<PlayerStat, object> PlayerAbilities => curPlayerAbility.CharacterAbilities;
    public event Action<IReadOnlyDictionary<PlayerStat, object>> OnAbilityChanged;
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnExpChanged;

    // 능력치 변경 프로퍼티
    public void IncreaseHp(int _amount) { curPlayerAbility.Hp += _amount; }
    public void IncreaseMp(int _amount) { curPlayerAbility.Mp += _amount; }

    public void SetHp(int _amount) => curPlayerAbility.Hp = _amount;

    public void SetMp(int _amount) => curPlayerAbility.Mp = _amount;

    public void ChangeMaxHp(int _maxHp) { curPlayerAbility.MaxHp = _maxHp; }
    public void ChangeMaxMp(int _maxMp) { curPlayerAbility.MaxMp = _maxMp; }

    public void IncreaseAddMaxHp(int _amount) { curPlayerAbility.AddMaxHp += _amount; }
    public void IncreaseAddMaxMp(int _amount) { curPlayerAbility.AddMaxMp += _amount; }

    public void ChangeAtk(int _atk) { curPlayerAbility.Atk = _atk; }
    public void IncreaseAddAtk(int _amount) { curPlayerAbility.AddAtk += _amount; }

    public void ChangeDef(int _def) { curPlayerAbility.Def = _def; }
    public void IncreaseAddDef(int _amount) { curPlayerAbility.AddDef += _amount; }

    public void ChangeAttackRange(float _attackRange) { curPlayerAbility.AttackRange = _attackRange; }

    public void ChangeLevel(int _level) { curPlayerAbility.Level = _level; }

    private void Awake()
    {
        Initial();
        ServiceLocator.Register<IPlayerAbilityManager>(this);
    }

    private void OnDisable()
    {
        curPlayerAbility.OnAbilityChanged -= OnAbilityChangedMethod;
    }

    //이벤트
    /// <summary>
    /// 능력치 변경 이벤트를 외부에 전달합니다.
    /// </summary>
    /// <param name="abilities"></param>
    public void OnAbilityChangedMethod(IReadOnlyDictionary<PlayerStat, object> abilities)
    {
        OnAbilityChanged?.Invoke(abilities);
    }

    /// <summary>
    /// CSV 데이터를 기반으로 플레이어 능력치를 설정합니다.
    /// </summary>
    public void SetPlayerAbility(int _playerLevel)
    {
        Dictionary<string, object> abilityDic = playerLevelInfo.GetDataByCode(_playerLevel.ToString());

        curPlayerAbility.MaxHp = int.Parse(abilityDic["HP"].ToString());
        curPlayerAbility.Hp = (int)PlayerAbilities[PlayerStat.TOTAL_MAXHP];//int.Parse(abilityDic["HP"].ToString());
        curPlayerAbility.MaxMp = int.Parse(abilityDic["MP"].ToString());
        curPlayerAbility.Mp = (int)PlayerAbilities[PlayerStat.TOTAL_MAXMP];//int.Parse(abilityDic["MP"].ToString());
        curPlayerAbility.Atk = int.Parse(abilityDic["ATK"].ToString());
        curPlayerAbility.Def = int.Parse(abilityDic["DEF"].ToString());
        curPlayerAbility.Level = int.Parse(abilityDic["LEVEL"].ToString());

        maxExp = int.Parse(abilityDic["NEEDEXP"].ToString());
    }

    /// <summary>
    /// 경험치를 추가하고, 필요 시 레벨업을 처리합니다.
    /// </summary>
    public void AddExp(int _exp)
    {
        currentExp += _exp;
        if (currentExp >= maxExp && curPlayerAbility.Level < maxLv)
        {
            LevelUp();
        }
        else
        {
            OnExpChanged?.Invoke(currentExp, maxExp);
        }
    }

    /// <summary>
    /// 경험치가 충분할 경우 레벨업을 반복적으로 수행합니다.
    /// </summary>
    private void LevelUp()
    {
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            SetPlayerAbility(curPlayerAbility.Level + 1);
        }

        OnLevelUp?.Invoke(curPlayerAbility.Level);
        Debug.Log("레벨 up " + curPlayerAbility.Level);

        OnExpChanged?.Invoke(currentExp, maxExp);
    }

    /// <summary>
    /// 능력치 및 경험치를 초기화합니다.
    /// </summary>
    public void Initial()
    {
        curPlayerAbility = Instantiate(curPlayerAbility);
        curPlayerAbility.Initial();
        curPlayerAbility.OnAbilityChanged += OnAbilityChangedMethod;

        currentExp = 0;
        playerLevelInfo = CSVReader.Read(playerLevelInfoTextAsset.text);
        SetPlayerAbility(1);

        OnExpChanged?.Invoke(0, maxExp);
    }
}
