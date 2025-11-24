using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

//플레이어 혹은 몬스터의 스킬과 일반 공격을 제어하는 핵심 컨트롤러입니다.
//스킬 시전, 콤보 공격, 애니메이션 트리거, 사운드 재생, 무기 트레일 처리 등
//전투 시스템의 흐름을 통합 관리합니다.
public class SkillController : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Skill[] normalSkills;  //일반 스킬배열
    private Skill curActionSkill;   //현재 실행중인 스킬

    //능력치
    private ISkillUser skillUser;   //이 스킬 사용자

    //연속공격
    private bool isNormalAttacking; //일반 공격중인지 확인합니다.
    private bool isComboEnableTime;   //콤보공격 가능한 시점인가? (해당 시점에 공격입력이 나오면 콤보공격 가능해진다)
    private bool isCombo;   //연속 공격을 하는가? (콤보공격이 가능한 시점에 공격 버튼누르면 true)
    private int combo;  //연속 공격 콤보

    //스킬공격
    private bool isCanCastSkill;    //지금 스킬을 쓸 수 있는지 확인합니다.
    private float remainCanCastSkillTime;   //몇 초 뒤에 스킬을 쓸 수 있는지 확인합니다.
    private bool isActive;  //지금 스킬 컨트롤러를 사용할수있는지 확인합니다.
                            //피격이나 상호작용할때에는 스킬을 쓸 수 없는 상황이므로 false가 됩니다.
    [SerializeField] private string animatorSkillTriggerName;   //애니메이터 파라미터(스킬시전 트리거)명
    [SerializeField] private string animatorSkillEndParam;  //애니메이터 파라미터(스킬끝날때 판정하는 bool형)이름
    [SerializeField] private string animatorSkillSpeedParam;  //애니메이터 파라미터(스킬 속도 float형)이름
    [SerializeField] private string aoSkillAnimationName;   //애니메이터 오버라이드에 설정된 원래 스킬이름

    //외부 클래스
    [SerializeField] private CharacterSoundController characterSoundController;

    //이벤트
    public event Action OnCastSkill;            //스킬 사용시 실행됩니다.
    public event Action OnCanCastSkillEnter;    //스킬을 쓸수있을때 실행됩니다.
    public event Action OnNormalAttackStart;    //일반 공격 시작시 실행됩니다.
    public event Action OnNormalAttackEnd;      //연속 공격이 끝날시 실행됩니다.

    private Action<int> onRequestMpChangeed;    //mp스킬 사용시 mp변경 요청 이벤트입니다.

    private Animator animator;
    private AnimatorOverrideController ao;
    private TrailRenderer weaponTrail;

    //이 속성이 TRUE일때 공격버튼 누르면 
    //현재 공격애니메이션이 끝난 후 다음 연속 공격이 시전됩니다.
    public bool IsComboEnableTime
    {
        get => isComboEnableTime;
        set => isComboEnableTime = value;
    }

    //외부 클래스에서 IsComboEnableTime가 true일때
    //공격 버튼 누르면 IsCombo = true로 바뀌도록 구현했습니다.
    public bool IsCombo
    {
        get => isCombo;
        set => isCombo = value;
    }

    private void Awake()
    {
        //애니메이터 초기화 및 스킬 사용 가능 상태 설정
        animator = GetComponent<Animator>();
        isCanCastSkill = true;
        isActive = true;
    }

    private void Update()
    {
        //스킬 컨트롤러가 활성화되어있지않으면 중단
        if (!isActive)
        {
            return;
        }

        //현재 일반 공격중이라면 중단
        if (isNormalAttacking)
        {
            return;
        }

        //스킬을 사용 할 수 있으면 중단
        if (isActive && isCanCastSkill)
        {
            return;
        }

        //위 세가지 상태가 아니면 쿨타임 감소 처리
        remainCanCastSkillTime -= Time.deltaTime;
        if (remainCanCastSkillTime < 0)
        {
            remainCanCastSkillTime = 0;
            isCanCastSkill = true;

            if (animatorSkillEndParam != string.Empty)
                animator.SetBool(animatorSkillEndParam, true);

            OnCanCastSkillEnter?.Invoke();
        }
    }

    //스킬 컨트롤러 초기화, 애니메이터 오버라이드 컨트롤러 객체 생성, 무기 트레일, MP 이벤트 설정
    public void Initial(ISkillUser _skillUser, TrailRenderer _weaponTrail, Action<int> _onMpRequestChanged = null)
    {
        skillUser = _skillUser;
        weaponTrail = _weaponTrail;
        onRequestMpChangeed = _onMpRequestChanged;

        if (animator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController))
        {
            ao = (AnimatorOverrideController)animator.runtimeAnimatorController;
        }
        else
        {
            ao = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = ao;
        }
    }

    //무기 트레일을 설정합니다.
    public void SetWeaponTrail(TrailRenderer _weaponTrail)
    {
        weaponTrail = _weaponTrail;
    }

    //스킬 컨트롤러 활성/비활성 설정합니다.
    //피격 시 비활성화 처리됩니다.
    public void SetActiveSkillController(bool _isActive)
    {
        isActive = _isActive;
        isCanCastSkill = _isActive;

        if (!_isActive)
        {
            remainCanCastSkillTime = 0;

            if (animatorSkillEndParam != string.Empty)
                animator.SetBool(animatorSkillEndParam, true);

            TrailOff();
            isNormalAttacking = false;

            ResetNormalAttack();
        }
    }

    //스킬을 시전합니다. (애니메이션, 사운드, MP 소모, 쿨타임 처리 포함)
    public void CastSkill(Skill _skill)
    {
        if (isActive && isCanCastSkill)
        {
            if ((int)skillUser.Ability()[PlayerStat.MP] >= _skill.MpCost && _skill.RemainCoolTime <= 0)
            {
                onRequestMpChangeed?.Invoke(-_skill.MpCost);    //mp변경요청 이벤트에 -(스킬 마나)인자를 전달합니다.

                curActionSkill = _skill;
                remainCanCastSkillTime = _skill.SkillCastingTime;
                animator.SetTrigger(animatorSkillTriggerName);
                ao[aoSkillAnimationName] = _skill.SkillAnimationClip;
                animator.SetFloat(animatorSkillSpeedParam, _skill.SkillAnimationSpeed);

                //스킬후 이동혹은 스킬사용이 가능한지 확인하는
                //애니메이터 파라미터값 false
                if (animatorSkillEndParam != string.Empty)
                    animator.SetBool(animatorSkillEndParam, false);

                _skill.OnStart(skillUser);
                isCanCastSkill = false;

                if (characterSoundController != null)
                {
                    if (!string.IsNullOrEmpty(curActionSkill.Sfx))
                        characterSoundController.PlayOneShotSFX(curActionSkill.Sfx);

                    if (!string.IsNullOrEmpty(curActionSkill.VoiceSound))
                        characterSoundController.Play(curActionSkill.VoiceSound);
                }

                OnCastSkill?.Invoke();
            }
        }
    }

    //일반 공격을 실행합니다. (콤보 처리 및 애니메이션 트리거 포함)
    public void NormalAttack()
    {
        if (isActive && isCanCastSkill)
        {
            ResetAllTrigger();

            if (combo == 0)
                animator.SetTrigger("Attack");
            else
                animator.SetTrigger("ComboAttack");

            combo++;

            if (normalSkills.Length > 0)
                curActionSkill = normalSkills[(combo - 1) % normalSkills.Length];
            else
                Debug.LogError("플레이어의 평타스킬을 등록하지 않았습니다.");

            curActionSkill.OnStart(skillUser);
            isNormalAttacking = true;
            isCanCastSkill = false;

            if (characterSoundController != null)
            {
                if (curActionSkill.Sfx != string.Empty)
                    characterSoundController.PlayOneShotSFX(curActionSkill.Sfx);

                if (curActionSkill.VoiceSound != string.Empty)
                    characterSoundController.Play(curActionSkill.VoiceSound);
            }

            OnNormalAttackStart?.Invoke();
        }
    }

    //모든 트리거 파라미터를 초기화합니다.
    private void ResetAllTrigger()
    {
        AnimatorControllerParameter[] triggerParams = animator.parameters.Where(acp => acp.type == AnimatorControllerParameterType.Trigger).ToArray();
        foreach (AnimatorControllerParameter triggerParam in triggerParams)
        {
            animator.ResetTrigger(triggerParam.name);
        }
    }

    #region Animation Event
    //애니메이션 중 공격 판정 시 스킬 효과를 적용합니다.
    public void InvokeSkillEffect()
    {
        if (curActionSkill != null)
            curActionSkill.Action(skillUser);
    }

    //무기 트레일 이펙트를 켭니다.
    private void TrailOn()
    {
        if (!isActive)
            return;

        if (weaponTrail != null)
            weaponTrail.enabled = true;
    }

    //무기 트레일 이펙트를 끕니다.
    private void TrailOff()
    {
        if (weaponTrail != null)
            weaponTrail.enabled = false;
    }

    //연속 공격

    //콤보 입력 가능 상태로 전환합니다.
    private void EnableComboInput()
    {
        isComboEnableTime = true;
    }

    //콤보 입력 불가능 상태로 전환합니다.
    private void DisableComboInput()
    {
        isComboEnableTime = false;
    }

    //다음 콤보 공격을 실행할지 판단합니다.
    private void CheckNextComboAttack()
    {
        if (isCombo)
        {
            isCombo = false;
            isCanCastSkill = true;
            NormalAttack();
        }
    }

    //일반 공격(연속 공격) 종료 처리합니다.
    private void NormalAttackEnd()
    {
        combo = 0;
        isCombo = false;
        isComboEnableTime = false;

        //일반 공격이 끝나면 바로 스킬을 쓸 수 있게합니다.
        remainCanCastSkillTime = 0;
        isCanCastSkill = true;
        isNormalAttacking = false;

        OnNormalAttackEnd?.Invoke();
    }

    //일반 공격 상태를 초기화합니다.
    private void ResetNormalAttack()
    {
        combo = 0;
        isCombo = false;
        isComboEnableTime = false;

        //일반 공격이 끝나면 바로 스킬을 쓸 수 있게합니다.
        remainCanCastSkillTime = 0;
        isCanCastSkill = true;
        isNormalAttacking = false;
    }
    #endregion

}
