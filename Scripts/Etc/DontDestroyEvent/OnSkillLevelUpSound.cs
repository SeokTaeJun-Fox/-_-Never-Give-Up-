using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSkillLevelUpSound : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private string sfxName;

    private void Awake()
    {
        skillManager.OnSkillLevelUp += PlayLevelUpSound;
    }

    private void PlayLevelUpSound(Skill _skill)
    {
        if (GlobalData.isGameStart)
        {
            soundManager.PlayOneShotSFX(sfxName);
        }
    }

    private void OnDestroy()
    {
        if (skillManager != null)
            skillManager.OnSkillLevelUp -= PlayLevelUpSound;
    }
}
