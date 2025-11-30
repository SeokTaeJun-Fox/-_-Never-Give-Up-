using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class CharacterAbility : ScriptableObject
{
    protected Dictionary<PlayerStat, object> characterAbilities = new Dictionary<PlayerStat, object>();
    public IReadOnlyDictionary<PlayerStat, object> CharacterAbilities => characterAbilities;

    [SerializeField] protected int maxHp;  //최대체력 원래값
    [SerializeField] protected int hp;     //현재hp
    [SerializeField] protected int maxMp;  //최대mp 원래값
    [SerializeField] protected int mp;     //현재mp

    [SerializeField] protected int atk;    //atk 원래값
    [SerializeField] protected int def;    //def 원래값

    protected int addMaxHp = 0;  //최대체력 추가치 (장비,버프등으로인한 추가치)
    protected int addMaxMp = 0;   //최대mp 추가치 (장비,버프등으로인한 추가치)
    protected int addAtk = 0;     //공격력 추가치 (장비,버프등으로인한 추가치)
    protected int addDef = 0;     //방어력 추가치 (장비,버프등으로인한 추가치)

    [SerializeField] protected int level;
    [SerializeField] protected float attackRange; //공격범위

    public virtual void Initial()
    {
        characterAbilities = new Dictionary<PlayerStat, object>();
        characterAbilities[PlayerStat.MAXHP] = maxHp;
        characterAbilities[PlayerStat.HP] = hp;
        characterAbilities[PlayerStat.MAXMP] = maxMp;
        characterAbilities[PlayerStat.MP] = mp;
        characterAbilities[PlayerStat.ATK] = atk;
        characterAbilities[PlayerStat.DEF] = def;

        characterAbilities[PlayerStat.LEVEL] = level;
        characterAbilities[PlayerStat.ATTACKRANGE] = attackRange;

        characterAbilities[PlayerStat.ADD_MAXHP] = addMaxHp;
        characterAbilities[PlayerStat.ADD_MAXMP] = addMaxMp;
        characterAbilities[PlayerStat.ADD_ATK] = addAtk;
        characterAbilities[PlayerStat.ADD_DEF] = addDef;

        characterAbilities[PlayerStat.TOTAL_ATK] = Mathf.Clamp(atk + addAtk, 1, int.MaxValue);
        characterAbilities[PlayerStat.TOTAL_DEF] = Mathf.Clamp(def + addDef, 1, int.MaxValue);
        characterAbilities[PlayerStat.TOTAL_MAXHP] = Mathf.Clamp(maxHp + addMaxHp, 1, int.MaxValue);
        characterAbilities[PlayerStat.TOTAL_MAXMP] = Mathf.Clamp(maxMp + addMaxMp, 1, int.MaxValue);
    }

    //능력치 반영합니다.
    public virtual void CopyAbility(CharacterAbility _characterAbility)
    {
        MaxHp = _characterAbility.MaxHp;
        MaxMp = _characterAbility.MaxMp;
        Hp = _characterAbility.Hp;
        Mp = _characterAbility.Mp;
        Atk = _characterAbility.Atk;
        Def = _characterAbility.Def;
        Level = _characterAbility.Level;
        AttackRange = _characterAbility.attackRange;
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value;
            characterAbilities[PlayerStat.MAXHP] = maxHp;
            characterAbilities[PlayerStat.TOTAL_MAXHP] = addMaxHp + maxHp;

            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, (int)characterAbilities[PlayerStat.TOTAL_MAXHP]);
            characterAbilities[PlayerStat.HP] = hp;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int MaxMp
    {
        get => maxMp;
        set
        {
            maxMp = value;
            characterAbilities[PlayerStat.MAXMP] = maxMp;
            characterAbilities[PlayerStat.TOTAL_MAXMP] = addMaxMp + maxMp;

            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int Mp
    {
        get => mp;
        set
        {
            mp = Mathf.Clamp(value, 0, (int)characterAbilities[PlayerStat.TOTAL_MAXMP]);
            characterAbilities[PlayerStat.MP] = mp;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int Atk
    {
        get => atk;
        set
        {
            atk = value;
            characterAbilities[PlayerStat.ATK] = atk;
            characterAbilities[PlayerStat.TOTAL_ATK] = atk + addAtk;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int Def
    {
        get => def;
        set
        {
            def = value;
            characterAbilities[PlayerStat.DEF] = def;
            characterAbilities[PlayerStat.TOTAL_DEF] = def + addDef;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int AddMaxHp
    {
        get => addMaxHp;
        set
        {
            addMaxHp = value;
            characterAbilities[PlayerStat.ADD_MAXHP] = addMaxHp;
            characterAbilities[PlayerStat.TOTAL_MAXHP] = addMaxHp + maxHp;

            if ((int)characterAbilities[PlayerStat.HP] > (int)characterAbilities[PlayerStat.TOTAL_MAXHP])
                Hp = (int)characterAbilities[PlayerStat.TOTAL_MAXHP];

            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int AddMaxMp
    {
        get => addMaxMp;
        set
        {
            addMaxMp = value;
            characterAbilities[PlayerStat.ADD_MAXMP] = addMaxMp;
            characterAbilities[PlayerStat.TOTAL_MAXMP] = addMaxMp + maxMp;

            if ((int)characterAbilities[PlayerStat.MP] > (int)characterAbilities[PlayerStat.TOTAL_MAXMP])
                Mp = (int)characterAbilities[PlayerStat.TOTAL_MAXMP];

            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int AddAtk
    {
        get => addAtk;
        set
        {
            addAtk = value;
            characterAbilities[PlayerStat.ADD_ATK] = addAtk;
            characterAbilities[PlayerStat.TOTAL_ATK] = atk + addAtk;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int AddDef
    {
        get => addDef;
        set
        {
            addDef = value;
            characterAbilities[PlayerStat.ADD_DEF] = addDef;
            characterAbilities[PlayerStat.TOTAL_DEF] = def + addDef;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public int Level
    {
        get => level;
        set
        {
            level = value;
            characterAbilities[PlayerStat.LEVEL] = level;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public float AttackRange
    {
        get => attackRange;
        set
        {
            attackRange = value;
            characterAbilities[PlayerStat.ATTACKRANGE] = attackRange;
            OnAbilityChanged?.Invoke(characterAbilities);
        }
    }

    public event Action<IReadOnlyDictionary<PlayerStat, object>> OnAbilityChanged;

    protected void InvokeOnAbilityChanged()
    {
        OnAbilityChanged?.Invoke(characterAbilities);
    }
}
