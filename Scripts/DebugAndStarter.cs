using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAndStarter : MonoBehaviour
{
    [SerializeField] private ShowUIWindowFactoryEvent factoryEvent;

    [SerializeField] private List<EquipmentItem> getEquips;
    [SerializeField] private List<EnchantItem> getEnchants;
    [SerializeField] private List<ConsumableItem> getConsumables;
    [SerializeField] private EquipmentItem defaultWeapon;
    private IItemManager itemManager;
    private IEquipmentManager equipmentManager;
    private IPlayerAbilityManager playerAbilityManager;

    private void Start()
    {
        StartCoroutine(Setting());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (var equip in getEquips)
            {
                itemManager.GetItem(equip, 1, true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            foreach (var enchant in getEnchants)
            {
                itemManager.GetItem(enchant, 1);
            }

            playerAbilityManager.AddExp(60);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            foreach (var consumable in getConsumables)
            {
                itemManager.GetItem(consumable, 3);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            factoryEvent.Raise(UIType.SHOP_WINDOW);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            itemManager.GetGold(10000);
        }
    }

    private IEnumerator Setting()
    {
        yield return null;
        yield return null;
        itemManager = ServiceLocator.GetService<IItemManager>();
        equipmentManager = ServiceLocator.GetService<IEquipmentManager>();
        playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();

        //EquipmentItem cloneDefault = Instantiate(defaultWeapon);
        //cloneDefault.ItemKey = "no99999";

        //itemManager.GetItem(cloneDefault, 1);
        //equipmentManager.Equip(defaultWeapon.EquipmentType, cloneDefault);
    }
}
