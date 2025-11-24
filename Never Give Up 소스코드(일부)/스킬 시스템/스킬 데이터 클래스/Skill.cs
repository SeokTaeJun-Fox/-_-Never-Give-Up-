using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//스킬 정보 및 실행 로직을 담고 있는 스크립터블 오브젝트입니다.
//시전 시 애니메이션, 사운드, 행동 목록을 기반으로 스킬을 처리합니다.
[CreateAssetMenu(fileName = "skill_", menuName = "Scriptable Object/PlayerSkill/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField] private string skillName;  //스킬명
    [SerializeField] private string skillId;    //고유한 스킬id
    [SerializeField] private Sprite skillSprite;    //스킬 아이콘
    [TextArea, SerializeField] private string skillDesc;    //스킬 설명
    [SerializeField] private int skillLevel;    //스킬 레벨
    [SerializeField] private int mpCost;    //MP 소모량
    [SerializeField] private float skillCooltime;   //스킬 쿨타임
    [SerializeField] private float skillCastingTime;    //스킬 시전시간
    [SerializeField] private AnimationClip skillAnimationClip;  //스킬 애니메이션 클립
    [SerializeField] private float skillAnimationSpeed = 1; //스킬 애니메이션 속도
    [SerializeField] private string sfx;        //스킬 효과음
    [SerializeField] private string voiceSound; //시전 음성
    [SerializeField] private List<SkillAction> skillActions;    //스킬 행동 목록

    private float remainCoolTime;  //남은 쿨타임
                                   //이 변수로인해 스크립터블 오브젝트를 복제해야 합니다.
    private bool isModifyRemainCoolTime;    //쿨타임 수정 여부

    public string SkillName => skillName;
    public string SkillId => skillId;
    public string SkillDesc => skillDesc;
    public int SkillLevel => skillLevel;
    public Sprite SkillSprite => skillSprite;
    public int MpCost => mpCost;
    public float SkillCooltime => skillCooltime;
    public float SkillCastingTime => skillCastingTime;
    public AnimationClip SkillAnimationClip => skillAnimationClip;
    public float SkillAnimationSpeed => skillAnimationSpeed;

    public bool IsModifyRemainCoolTime
    {
        get => isModifyRemainCoolTime;
        set => isModifyRemainCoolTime = value;
    }

    public float RemainCoolTime
    {
        get => remainCoolTime;
        set
        {
            remainCoolTime = Mathf.Clamp(value, 0, float.MaxValue);
        }
    }

    public string Sfx { get => sfx; }
    public string VoiceSound { get => voiceSound; }

    //이 스킬애니메이션이 실행되자마자 호출됩니다.
    //각 SkillAction의 OnStart를 실행하고, 쿨타임을 초기화합니다.
    public void OnStart(ISkillUser _user)
    {
        foreach (var skillAction in skillActions)
            skillAction.OnStart(_user);

        if(isModifyRemainCoolTime)
            remainCoolTime = skillCooltime;
    }

    //특정 시점에 스킬의 실제 효과를 실행합니다.
    //SkillAction의 Action을 호출하여 대상에게 효과를 적용합니다.
    public void Action(ISkillUser _user)
    {
        foreach (SkillAction action in skillActions)
        {
            action.Action(_user);
        }
    }
}
