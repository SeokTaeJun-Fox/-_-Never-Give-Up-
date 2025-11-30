using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpSound : MonoBehaviour
{
    [SerializeField] private string sfxName;
    [SerializeField] private CharacterSoundController characterSoundController;
    private IPlayerAbilityManager playerAbilityManager;

    private void Awake()
    {
        playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();

        playerAbilityManager.OnLevelUp += PlayLevelUpSound;
    }

    private void PlayLevelUpSound(int _level)
    {
        if(characterSoundController != null)
            characterSoundController.PlayOneShotSFX(sfxName);
    }

    private void OnDestroy()
    {
        if (playerAbilityManager != null)
            playerAbilityManager.OnLevelUp -= PlayLevelUpSound;
    }
}
