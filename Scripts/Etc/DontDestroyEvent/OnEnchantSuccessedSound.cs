using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnchantSuccessedSound : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private string sfxName;

    private void Awake()
    {
        equipmentManager.OnEnchant += PlayEnchantSound;
    }

    private void PlayEnchantSound(EquipmentItem _equipmentItem, EnchantItem _enchantItem)
    {
        if (GlobalData.isGameStart)
        {
            soundManager.PlayOneShotSFX(sfxName);
        }
    }

    private void OnDestroy()
    {
        if (equipmentManager != null)
            equipmentManager.OnEnchant -= PlayEnchantSound;
    }
}
