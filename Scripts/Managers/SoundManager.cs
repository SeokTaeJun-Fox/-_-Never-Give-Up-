using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private string sfxPath;
    [SerializeField] private string bgmPath;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioSource bgmPlayer;

    [SerializeField] private string sfxVolumePrebsName;
    [SerializeField] private string bgmVolumePrebsName;
    [SerializeField] private float defaultSfxVolume;
    [SerializeField] private float defaultBgmVolume;

    public static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
    private List<string> playListFrame = new List<string>();

    public static float SfxVolume => sfxVolume;

    public static float BgmVolume => bgmvolume;

    public static string SfxPath;
    public static string BgmPath;

    private static float bgmvolume;
    private static float sfxVolume;
    private static float masterVolume;

    private void Awake()
    {
        SfxPath = sfxPath;
        BgmPath = bgmPath;

        ServiceLocator.Register<ISoundManager>(this);

        if (PlayerPrefs.HasKey(sfxVolumePrebsName))
        {
            float sfxVolume = PlayerPrefs.GetFloat(sfxVolumePrebsName);
            SetSFXVolume(sfxVolume);
        }
        else
        {
            SetSFXVolume(defaultSfxVolume);
        }

        if (PlayerPrefs.HasKey(bgmVolumePrebsName))
        {
            float bgmVolume = PlayerPrefs.GetFloat(bgmVolumePrebsName);
            SetBGMVolume(bgmVolume);
        }
        else
        {
            SetBGMVolume(defaultBgmVolume);
        }
    }

    private void LateUpdate()
    {
        if(playListFrame.Count > 0)
            playListFrame.Clear();
    }

    public void PlayBGM(string _bgm, float _masterVolume = 1f)
    {
        masterVolume = _masterVolume;

        var clip = Resources.Load<AudioClip>(bgmPath + _bgm);
        if (clip != null)
        { 
            bgmPlayer.clip = clip;
            bgmPlayer.volume = bgmvolume * _masterVolume;
            bgmPlayer.Play();
        }
    }

    public void PlayBGM()
    {
        if(bgmPlayer.clip != null)
            bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer != null)
            bgmPlayer.Stop();
    }

    public void PlayOneShotSFX(string _sfx)
    {
        if (playListFrame.Exists(x => x == _sfx))
            return;

        AudioClip clip = null;

        if (cache.ContainsKey(_sfx))
        {
            clip = cache[_sfx];
        }
        else
        {
            clip = Resources.Load<AudioClip>(sfxPath + _sfx);
            cache.Add(clip.name, clip);
        }

        if (clip != null)
        {
            sfxPlayer.PlayOneShot(clip);
            playListFrame.Add(_sfx);
        }
    }

    public void SetBGMVolume(float _volume)
    {
        bgmvolume = _volume;
        bgmPlayer.volume = bgmvolume * masterVolume;
    }

    public void SetSFXVolume(float _volume)
    {
        sfxVolume = _volume;
        sfxPlayer.volume = _volume;
    }

  
}
