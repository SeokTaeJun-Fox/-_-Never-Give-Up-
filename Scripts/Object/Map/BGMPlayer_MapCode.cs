using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGMPlayer_MapCode : MonoBehaviour
{
    [SerializeField] List<BGM_MapCodeInfo> infos;
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
        var info = infos.FirstOrDefault(x => x.mapCode == GlobalData.curMapCode);

        if (soundManager != null && info != null)
        {
            soundManager.PlayBGM(info.bgmName, info.masterVolume);
        }
    }

    private void OnDestroy()
    {
        if (soundManager != null)
            soundManager.StopBGM();
    }
}

[System.Serializable]
public class BGM_MapCodeInfo
{
    public string bgmName;
    public float masterVolume;
    public int mapCode;
}
