using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour, ICharacterSoundController
{
    [SerializeField] private AudioSource sfxPlayer;

    public void PlayOneShotSFX(string _sfx)
    {
        if (string.IsNullOrEmpty(_sfx))
            return;

        AudioClip clip = null;
        sfxPlayer.volume = SoundManager.SfxVolume;

        if (SoundManager.cache.ContainsKey(_sfx))
        {
            clip = SoundManager.cache[_sfx];
        }
        else
        {
            clip = Resources.Load<AudioClip>(SoundManager.SfxPath + _sfx);
            SoundManager.cache.Add(clip.name, clip);
        }

        if (clip != null)
        {
            sfxPlayer.volume = SoundManager.SfxVolume;
            sfxPlayer.PlayOneShot(clip);
        }
    }

    public void Play(string _sfx)
    {
        if (_sfx == string.Empty)
            return;

        AudioClip clip = null;
        sfxPlayer.volume = SoundManager.SfxVolume;

        if (SoundManager.cache.ContainsKey(_sfx))
        {
            clip = SoundManager.cache[_sfx];
        }
        else
        {
            clip = Resources.Load<AudioClip>(SoundManager.SfxPath + _sfx);
            SoundManager.cache.Add(clip.name, clip);
        }

        if (clip != null)
        {
            sfxPlayer.volume = SoundManager.SfxVolume;
            sfxPlayer.clip = clip;
            sfxPlayer.Play();
        }
    }

    public void Stop()
    {
        sfxPlayer.Stop();
    }
}
