using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 플레이어의 스킬을 관리하는 클래스입니다.
/// 스킬 획득, 레벨업, 스킬 포인트 사용 등을 처리하며합니다.
/// SkillBundle에있는 스킬 데이터 오브젝트를 사용하여 스킬 시스템을 처리합니다.
/// </summary>
public class SkillManager : MonoBehaviour, ISkillManager
{
    [SerializeField] private SkillBundle playerSkillBundle; //스킬 그룹 데이터

    public event Action<Skill> OnSkillLevelUp;  //스킬 레벨업 이벤트
    public event Action<int> OnSkillPointUse;   //스킬 포인트 사용 이벤트

    private Dictionary<string, Skill> currentSkillDic;    //현재 보유 중인 스킬 정보

    public int GetSkillPoint => skillPoint; //현재 보유한 스킬 포인트
    public int SetSkillPoint { set => skillPoint = value; }

    private int skillPoint;

    //이 클래스에 필요한 인터페이스들
    IPlayerAbilityManager playerAbilityManager;

    private void Awake()
    {
        playerSkillBundle = playerSkillBundle.Clone();  //스킬 데이터 복제

        currentSkillDic = new Dictionary<string, Skill>();

        //현재 스킬 레벨 데이터를 세팅합니다.
        foreach (var levelGroup in playerSkillBundle.SkillLevelGroups)
        {
            currentSkillDic.Add(levelGroup.SkillId, null);
        }

        //서비스 로케이터 등록
        ServiceLocator.Register<ISkillManager>(this);

        skillPoint = 5; //초기 스킬 포인트(게임을 새로시작할때 제공되는 스킬포인트입니다)
    }

    private void Start()
    {
        playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();
        if (playerAbilityManager != null)
        {
            playerAbilityManager.OnLevelUp += OnPlayerLevelUp;
        }
    }

    //특정 ID와 레벨의 스킬을 반환합니다.
    public Skill GetSkill(string _skillId, int level)
    {
        //해당 스킬 ID를 가진 그룹을 찾습니다.
        var skillLevelInfo = playerSkillBundle.FindLevelGroup(_skillId);
        if (skillLevelInfo != null)
        {
            if (level <= 0)
            {
                return skillLevelInfo.Skills.Find(x => x.SkillLevel == 1);
            }
            else
            {
                return skillLevelInfo.Skills.Find(x => x.SkillLevel == level);
            }
        }
        else
        {
            return null;
        }
    }

    //특정 스킬을 지정된 레벨로 보유합니다.
    public void PossessSkill(string _skillId, int level)
    {
        Skill possessedSkill = GetSkill(_skillId, level);
        if (currentSkillDic.ContainsKey(_skillId))
        {
            currentSkillDic[_skillId] = possessedSkill;
        }
        else
            currentSkillDic.Add(_skillId, possessedSkill);
    }

    //스킬의 최대 레벨을 반환합니다.
    public int GetMaxLevel(string _skillId)
    {
        //해당 스킬 ID를 가진 그룹을 찾습니다.
        var skillLevelInfo = playerSkillBundle.FindLevelGroup(_skillId);
        if (skillLevelInfo != null)
        {
            //해당 그룹 내 스킬들 중 가장 높은 레벨을 반환합니다.
            //해당 그룹은 레벨만 다르고 스킬은 같습니다.
            return skillLevelInfo.Skills.Max(x => x.SkillLevel);
        }
        else
        {
            return 0;
        }
    }

    //현재 보유 중인 스킬 정보를 반환합니다.
    public IReadOnlyDictionary<string, Skill> GetCurrentSkill()
    {
        return currentSkillDic;
    }

    //보유 스킬을 레벨업합니다.
    public void SkillLevelup(string _skillId, bool _isUseSkillPoint = true)
    {
        // 현재 플레이어가 보유한 스킬 딕셔너리에서 해당 스킬을 가져옵니다.
        Skill curSkill = currentSkillDic[_skillId];

        int curLevel;
        if (curSkill == null)
        {
            curLevel = 0;
        }
        else
        {
            curLevel = curSkill.SkillLevel;
        }

        //해당 스킬 ID를 가진 그룹을 찾습니다.
        var skillLevelInfo = playerSkillBundle.FindLevelGroup(_skillId);
        if (skillLevelInfo != null)
        {
            //스킬 최대 레벨이 현재 레벨보다 낮다면 해당 보유 스킬을 레벨업합니다.
            if (curLevel < GetMaxLevel(_skillId))
            { 
                //다음 레벨의 스킬을 불러옵니다.
                Skill upgradeSkill = skillLevelInfo.Skills.Find(x => x.SkillLevel == curLevel+1);

                //보유 스킬을 교체합니다.
                currentSkillDic[_skillId] = upgradeSkill;

                OnSkillLevelUp?.Invoke(upgradeSkill);

                if (_isUseSkillPoint)
                {
                    skillPoint--;
                    OnSkillPointUse?.Invoke(skillPoint);
                }
            }
        }
    }

    //이벤트 처리
    private void OnPlayerLevelUp(int _level)
    {
        skillPoint += 5;
    }

    private void OnDestroy()
    {
        if (playerAbilityManager != null)
        {
            playerAbilityManager.OnLevelUp -= OnPlayerLevelUp;
        }
    }

    //현재 스킬 레벨 데이터를 초기화합니다.
    public void Initial()
    {
        currentSkillDic.Clear();
        foreach (var levelGroup in playerSkillBundle.SkillLevelGroups)
        {
            currentSkillDic.Add(levelGroup.SkillId, null);
        }

        skillPoint = 5;
    }
}
