using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitilizer : MonoBehaviour
{
    //이 클래스에 필요한 스크립터블 오브젝트들
    [SerializeField] private ConsumableItemHotKeyInfo itemHotKeyInfo;
    [SerializeField] private SkillHotKeyInfo skillHotKeyInfo;

    [SerializeField] private DataInitialRequestEvent initialRequestEvent;

    //이 클래스에 필요한 인터페이스들
    private IPlayerAbilityManager playerAbilityManager;
    private IEquipmentManager equipmentManager;
    private IItemManager itemManager;
    private ISkillManager skillManager;
    private IBuffController buffController;

    private void Awake()
    {
        initialRequestEvent.OnDataInitialRequest += DataInitial;
    }

    private void Start()
    {
        playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();
        equipmentManager = ServiceLocator.GetService<IEquipmentManager>();
        itemManager = ServiceLocator.GetService<IItemManager>();
        skillManager = ServiceLocator.GetService<ISkillManager>();
        buffController = ServiceLocator.GetService<IBuffController>();
    }

    public void DataInitial()
    {
        buffController?.ClearAllBuff();
        equipmentManager?.Initial();
        playerAbilityManager?.Initial();
        itemHotKeyInfo.ClearData();
        itemManager?.Initial();

        skillHotKeyInfo.ClearData();
        skillManager?.Initial();

        QuestSystem.Instance.Initial();

        GlobalData.isSeePrologue = false;
    }

    private void OnDestroy()
    {
        initialRequestEvent.OnDataInitialRequest -= DataInitial;
    }
}
