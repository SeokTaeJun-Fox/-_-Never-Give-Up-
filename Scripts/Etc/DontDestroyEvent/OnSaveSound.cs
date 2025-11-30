using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSaveSound : MonoBehaviour
{
    [SerializeField] private SaveLoadManager saveLoadManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private string sfxName;

    private void Awake()
    {
        saveLoadManager.OnSave += PlaySaveSound;
    }

    private void PlaySaveSound()
    {
        soundManager.PlayOneShotSFX(sfxName);
    }

    private void OnDestroy()
    {
        if (saveLoadManager != null)
            saveLoadManager.OnSave -= PlaySaveSound;
    }
}
