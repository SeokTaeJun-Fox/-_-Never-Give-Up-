using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoVillageAfterPlayerDead : MonoBehaviour
{
    [SerializeField] private PlayerAbilityManager playerAbilityManager;

    [SerializeField] private LoadSceneRequestEvent loadSceneRequestEvent;

    public void GoVillage()
    {
        StartCoroutine(CoGoVillage());
    }

    private IEnumerator CoGoVillage()
    {
        yield return new WaitForSeconds(2);

        playerAbilityManager.SetHp(1);
        loadSceneRequestEvent.Raise(LoadSceneType.VILLAGE_SCENE, 0);
    }
}
