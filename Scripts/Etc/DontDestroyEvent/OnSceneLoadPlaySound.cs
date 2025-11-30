using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoadPlaySound : MonoBehaviour
{
    [SerializeField] private string sfxName;
    private ISoundManager soundManager;
    private ISceneLoader sceneLoader;

    private void Start()
    {
        soundManager = ServiceLocator.GetService<ISoundManager>();
        sceneLoader = ServiceLocator.GetService<ISceneLoader>();

        if (sceneLoader != null)
        {
            sceneLoader.OnSceneLoaded += PlaySFX;
        }
    }

    public void PlaySFX()
    {
        if (soundManager != null)
        {
            soundManager.PlayOneShotSFX(sfxName);
        }
    }

    private void OnDestroy()
    {
        if (sceneLoader != null)
        {
            sceneLoader.OnSceneLoaded -= PlaySFX;
        }
    }
}
