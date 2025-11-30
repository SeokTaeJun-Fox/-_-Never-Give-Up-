using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundManager
{
    void PlayBGM(string _bgm, float _masterVolume = 1f);
    void PlayBGM();
    void StopBGM();

    void PlayOneShotSFX(string _sfx);

    void SetSFXVolume(float _volume);
    void SetBGMVolume(float _volume);

    //float SfxVolume { get; }
    //float BgmVolume { get; }
}
