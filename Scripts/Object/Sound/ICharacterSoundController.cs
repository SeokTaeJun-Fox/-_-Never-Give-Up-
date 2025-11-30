using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterSoundController
{
    void PlayOneShotSFX(string _sfx);
    void Play(string _sfx);
    void Stop();
}
