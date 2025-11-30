using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRandomIdle : MonoBehaviour
{
    [Header("Random Idle")]
    //플레이어가 움직이지 않을때부터 랜덤 아이들 실행까지 시간
    [SerializeField] private float randomIdleAriseTime;

    [SerializeField] private string idleClipName;     //아이들 애니메이션 클립 이름

    [Header("Animator")]
    [SerializeField] private string waitAnimationTriggerName;   //랜덤 아이들 실행 애니메이션 트리거명

    private Animator animator;

    private bool canRandomIdle; //랜덤 아이들을 할 수 있는지 확인합니다.

    public bool CanRandomIdle
    {
        set => canRandomIdle = value;
    }

    //랜덤 아이들 실행까지 남은시간
    private float remainRandimIdleAriseTime;

    private void Awake()
    {
        canRandomIdle = true;
        animator = GetComponent<Animator>();
        remainRandimIdleAriseTime = randomIdleAriseTime; //랜덤 아이들 초기화
    }

    private void Update()
    {
        //1. 랜덤 아이들
        if (canRandomIdle)
        {
            if (GetCurrentPlayingAnimationName().Equals(idleClipName))
            {
                remainRandimIdleAriseTime -= Time.deltaTime;
                if (remainRandimIdleAriseTime <= 0)
                {
                    PlayRandomIdle();
                    remainRandimIdleAriseTime = randomIdleAriseTime;
                }
            }
            else
            {
                remainRandimIdleAriseTime = randomIdleAriseTime;
            }
        }
        else
            remainRandimIdleAriseTime = randomIdleAriseTime; //랜덤 아이들 초기화
    }

    //랜덤 아이들을 실행합니다.
    private void PlayRandomIdle()
    {
        Utility.ResetAllTrigger(animator);
        animator.SetTrigger(waitAnimationTriggerName);
    }


    //현재 실행하고있는 애니메이션이름을 불러옵니다.
    private string GetCurrentPlayingAnimationName()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length != 0)
            return clipInfo[0].clip.name;
        else
            return "";
    }
}
