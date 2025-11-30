using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private string bgmName;
    [SerializeField] private float masterVolume;
    [SerializeField] private float delayPlayTime;
    private ISoundManager soundManager;

    private void Awake()
    {
        soundManager = ServiceLocator.GetService<ISoundManager>();
        StartCoroutine(CoPlayBgm());
    }

    private IEnumerator CoPlayBgm()
    { 
        yield return new WaitForSeconds(delayPlayTime);
        if (soundManager != null)
        {
            soundManager.PlayBGM(bgmName, masterVolume);
        }
    }

    private void OnDestroy()
    {
        if (soundManager != null)
            soundManager.StopBGM();
    }
}
