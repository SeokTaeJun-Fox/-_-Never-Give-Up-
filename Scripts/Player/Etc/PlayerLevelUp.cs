using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    [SerializeField] private GameObject preb;
    [SerializeField] private Transform particlePos;
    [SerializeField] private Transform parent;
    [SerializeField] private float destroyParticleTime;
    private IPlayerAbilityManager abilityManager;

    private void Awake()
    {
        abilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();
        if (abilityManager != null)
        {
            abilityManager.OnLevelUp += PlayLevelUpParticle;
        }
    }

    private void OnDestroy()
    {
        if (abilityManager != null)
        {
            abilityManager.OnLevelUp -= PlayLevelUpParticle;
        }
    }

    public void PlayLevelUpParticle(int _level)
    {
        var go = Instantiate(preb, particlePos);
        go.transform.parent = parent;

        go.GetComponent<ParticleSystem>().Play();
    
        Destroy(go, destroyParticleTime);
    }
}
