using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSoundPlayer : MonoBehaviour
{
    [SerializeField] private string sfxName;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private bool isPlayOnEnable;
    private ISoundManager soundManager;

    private void Awake()
    {
        soundManager = ServiceLocator.GetService<ISoundManager>();
    }

    private void OnEnable()
    {
        if (soundManager != null && isPlayOnEnable)
        {
            sfxSource.volume = SoundManager.SfxVolume;
            soundManager.PlayOneShotSFX(sfxName);
        }
    }

    public void PlayOneShot()
    {
        if (soundManager != null)
        {
            sfxSource.volume = SoundManager.SfxVolume;
            soundManager.PlayOneShotSFX(sfxName);
        }
    }
}
