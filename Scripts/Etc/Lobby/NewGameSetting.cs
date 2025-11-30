using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임이 새로 시작할때 세팅해주는 클래스입니다.
public class NewGameSetting : MonoBehaviour
{
    [SerializeField] private EquipmentItem defaultWeapon;
    private IItemManager itemManager;
    private IEquipmentManager equipmentManager;

    public void Setting()
    {
        itemManager = ServiceLocator.GetService<IItemManager>();
        equipmentManager = ServiceLocator.GetService<IEquipmentManager>();

        if (itemManager == null || equipmentManager == null)
        {
            Debug.LogWarning("ItemManager 혹은 EquipManager가 존재하지 않으므로 새 게임에 필요한 아이템을 획득하지 못했습니다.");
            return;
        }

        EquipmentItem cloneDefault = Instantiate(defaultWeapon);
        cloneDefault.ItemKey = "no99999";

        itemManager.GetItem(cloneDefault, 1);
        equipmentManager.Equip(defaultWeapon.EquipmentType, cloneDefault);
    }
}
