using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParticleAttacher
{
    void PlayParticleOneShot(string _particlePoolName, Vector3 _localPos, bool _isCancelable, float _masterScale);
    void RemoveAllCancelableParticle();
    void PlayParticleOn(string _particlePoolName, string _code, float _masterScale);
    void PlayParticleOff(string _code);
}
