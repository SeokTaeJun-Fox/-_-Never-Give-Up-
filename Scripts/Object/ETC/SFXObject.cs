using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXObject : MonoBehaviour
{
    private ISoundManager soundManager;

    private void Awake()
    {
        soundManager = ServiceLocator.GetService<ISoundManager>();
    }

    private void Start()
    {
        if(soundManager == null)
            soundManager = ServiceLocator.GetService<ISoundManager>();
    }

    public void PlaySFX(string _sfxName)
    {
        if (soundManager != null)
        {
            soundManager.PlayOneShotSFX(_sfxName);
        }
    }
}
