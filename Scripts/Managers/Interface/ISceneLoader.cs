using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneLoader
{
    event Action<float> OnProgressChanged;
    event Action OnSceneLoaded;
    event Action OnLoadComplete;
    float Progress { get; }
}
