using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버프 정보를 담고 있는 스크립터블 오브젝트입니다.
//시작/종료 효과와 지속 효과를 분리하여 관리하며, 파티클 및 사운드 연출도 포함됩니다.
[CreateAssetMenu(fileName = "Buff_", menuName = "Scriptable Object/Buff/Buff")]
public class Buff : ScriptableObject
{
    [SerializeField] private string buffCode;   //버프 고유 코드
    [SerializeField] private Category category; //버프 카테고리
    [SerializeField] private string buffName;   //버프 이름
    [SerializeField, TextArea] private string buffDesc; //버프 설명
    [SerializeField] private Sprite icon;   //버프 아이콘
    [SerializeField] private float buffTime;    //버프 지속 시간
    [SerializeField] private bool isTick;   //지속형 버프 여부(ex : 화상,중독시 초당 데미지)
    [SerializeField] private float tickTime;    //지속 효과 주기
    [SerializeField] private string particlePoolName;   //파티클 오브젝트 풀 키
    [SerializeField] private float particleMasterScale; //파티클 마스터 사이즈
                                                        //오브젝트별 파티클 크기 * 파티클 마스터 사이즈

    [SerializeField] private string sfx;    //효과음 사운드 키

    [SerializeField] private List<BuffEffect_StartEnd> startEffect; //시작/종료 효과 리스트
    [SerializeField] private List<BuffEffect_Tick> tickEffect;  //지속 효과 리스트

    public string BuffCode { get => buffCode; }
    public Category Category { get => category; }
    public string BuffName { get => buffName; }
    public string BuffDesc { get => buffDesc; }
    public Sprite Icon { get => icon; }
    public float BuffTime { get => buffTime; }
    public bool IsTick { get => isTick; }
    public float TickTime { get => tickTime; }
    public string Sfx { get => sfx; }

    //버프 시작 시 효과 적용 및 파티클 실행
    public void OnStart(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        foreach (var effect in startEffect)
            effect.TakeEffect(_user, _providerAbility);

        _user.PlayParticleOn(particlePoolName, BuffCode, particleMasterScale);
    }

    //버프 지속 효과 실행
    //(isTick = true일 때 주기적으로 호출)
    public void OnTick(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        foreach (var effect in tickEffect)
            effect.Tick(_user, _providerAbility);
    }

    //버프 종료 시 효과 제거 및 파티클 종료
    public void OnEnd(IBuffUser _user, IReadOnlyDictionary<PlayerStat, object> _providerAbility)
    {
        foreach (var effect in startEffect)
            effect.EndEffect(_user, _providerAbility);

        _user.PlayParticleOff(BuffCode);
    }
}
