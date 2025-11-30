using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUnEquipSuccessedSound : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private string sfxName;

    private void Awake()
    {
        equipmentManager.OnEquipChanged += PlayEquipUnEquipSound;
    }

    private void PlayEquipUnEquipSound(EquipmentType _type, EquipmentItem _equipmentItem)
    {
        if (GlobalData.isGameStart)
        {
            soundManager.PlayOneShotSFX(sfxName);
        }
    }

    private void OnDestroy()
    {
        if(equipmentManager != null)
            equipmentManager.OnEquipChanged -= PlayEquipUnEquipSound;
    }
}
