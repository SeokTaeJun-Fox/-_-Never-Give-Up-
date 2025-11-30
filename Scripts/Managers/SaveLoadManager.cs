using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SaveLoadManager : MonoBehaviour, ISaveLoadManager
{
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private EnchantItemDataBase enchantItemDataBase;
    [SerializeField] private string fileName;
    string path;

    public event Action OnSave;

    //초기화에 사용할 스크립터블 오브젝트
    [SerializeField] private ConsumableItemHotKeyInfo itemHotKeyInfo;
    [SerializeField] private SkillHotKeyInfo skillHotKeyInfo;

    //이 클래스에 필요한 데이터 인터페이스들
    private IItemManager itemManager;
    private IEquipmentManager equipmentManager;
    private ISkillManager skillManager;
    private IPlayerAbilityManager playerAbilityManager;

    private void Awake()
    {
#if UNITY_EDITOR
        path = Path.Combine(Application.dataPath, fileName);
#else
        path = Path.Combine(Application.persistentDataPath, fileName);
#endif

        ServiceLocator.Register<ISaveLoadManager>(this);
    }

    private void Start()
    {
        itemManager = ServiceLocator.GetService<IItemManager>();
        equipmentManager = ServiceLocator.GetService<IEquipmentManager>();
        skillManager = ServiceLocator.GetService<ISkillManager>();
        playerAbilityManager = ServiceLocator.GetService<IPlayerAbilityManager>();
    }

    public void Save()
    {
        var saveData = CreateSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

        OnSave?.Invoke();
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            var saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            ApplyData(saveData);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
        }
    }

    public void Delete()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public bool IsFileExist()
    {
        return File.Exists(path);
    }

    //저장 데이터 생성 메소드
    private SaveData CreateSaveData()
    {
        var saveData = new SaveData();
        saveData = InputPlayerStat(saveData);
        saveData = InputPossessedItemData(saveData);
        saveData = InputEquippedItemData(saveData);
        saveData = InputEnchantListData(saveData);
        saveData = InputConsumableItemHotKeyData(saveData);
        saveData = InputPossessedSkillData(saveData);
        saveData = InputSkillHotKeyData(saveData);

        saveData = InputMoneyAndSkillPoint(saveData);
        saveData = InputGlobalData(saveData);
        saveData = InputQuestSaveData(saveData);
        return saveData;
    }

    //저장 데이터를 본 게임에 적용시키는 메소드
    private void ApplyData(SaveData _data)
    {
        ApplyPlayerStat(_data);
        ApplyPossessedItemData(_data);
        ApplyEquippedItemData(_data);
        ApplyEnchantListData(_data);
        ApplyConsumableItemHotKeyData(_data);
        ApplyPossessedSkillData(_data);
        ApplySkillHotKeyData(_data);

        ApplyMoneyAndSkillPoint(_data);
        ApplyGlobalData(_data);
        ApplyQuestSaveData(_data);
    }

    #region 세이브 데이터 제작 과정 메소드

    //보유하고있는 아이템을 SaveData에 입력합니다.  (1)
    private SaveData InputPossessedItemData(SaveData _data)
    {
        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager클래스가 존재하지 않습니다.");
            return _data;
        }

        var itemStacks = itemManager.ItemStacks;
        foreach (var itemStack in itemStacks)
        {
            _data.possessedItemDatas.Add(new PossessedItemData(itemStack.item.ItemKey, itemStack.item.ItemNum, itemStack.amount));
        }

        return _data;
    }

    //착용된 장비아이템 목록을 SaveData에 입력합니다.    (2)
    private SaveData InputEquippedItemData(SaveData _data)
    {
        if (equipmentManager == null)
        {
            Debug.LogWarning("IEquipmentManager클래스가 존재하지 않습니다.");
            return _data;
        }

        var equippedItemInfo = equipmentManager.EquippedItemDic;
        foreach (var item in equippedItemInfo.Values)
        {
            if(item != null)
                _data.equipList.Add(item.ItemKey);
        }

        return _data;
    }

    //보유하고있는 장비아이템의 강화 목록을 SaveData에 입력합니다. (3)
    private SaveData InputEnchantListData(SaveData _data)
    {
        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager클래스가 존재하지 않습니다.");
            return _data;
        }

        var itemStacks = itemManager.ItemStacks.Where(x => x.item is EquipmentItem)
                                                .Select(x => x.item as EquipmentItem)
                                                .ToList();

        string enchantedItemKey = "";
        foreach (var item in itemStacks)
        {
            enchantedItemKey = item.ItemKey;
            foreach (var enchant in item.EnchantList)
            {
                _data.equipmentEnchantList.Add(new EquipmentEnchantData(enchantedItemKey, enchant.ItemNum));
            }
        }

        return _data;
    }

    //단축키에 등록된 소비아이템 데이터를 입력합니다. (4)
    private SaveData InputConsumableItemHotKeyData(SaveData _data) 
    {
        if (itemHotKeyInfo == null)
        {
            Debug.LogWarning("ConsumableItemHotKeyInfo스크립터블 오브젝트를 지정하지 않았습니다.");
            return _data;
        }

        for (int i = 0; i < itemHotKeyInfo.Items.Count; i++)
        {
            if (itemHotKeyInfo.Items[i] == null)
            {
                _data.hotKeyItemList.Add(string.Empty);
            }
            else
            {
                _data.hotKeyItemList.Add(itemHotKeyInfo.Items[i].ItemKey);
            }
        }

        return _data;
    }

    //현재 플레이어 스킬정보 데이터를 입력합니다.  (5)
    private SaveData InputPossessedSkillData(SaveData _data)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("skillManager클래스가 존재하지 않습니다.");
            return _data;
        }

        var possessedSkillDataDic = skillManager.GetCurrentSkill();
        foreach (var skill in possessedSkillDataDic.Values)
        {
            if (skill == null)
                continue;

            var skillData = new PossessedSkillData(skill.SkillId, skill.SkillLevel);
            _data.possessedSkillList.Add(skillData);
        }

        return _data;
    }

    //단축키에 등록된 스킬 데이터를 입력합니다.   (6)
    private SaveData InputSkillHotKeyData(SaveData _data)
    {
        if (skillHotKeyInfo == null)
        {
            Debug.LogWarning("SkillHotKeyInfo스크립터블 오브젝트를 지정하지 않았습니다.");
            return _data;
        }

        for (int i = 0; i < skillHotKeyInfo.Infos.Count; i++)
        {
            if (skillHotKeyInfo.Infos[i].skill == null)
            {
                _data.hotKeySkillList.Add(null);
            }
            else
            {
                string id = skillHotKeyInfo.Infos[i].skill.SkillId;
                int level = skillHotKeyInfo.Infos[i].skill.SkillLevel;
                var hotKeyInfo = new PossessedSkillData(id, level);
                _data.hotKeySkillList.Add(hotKeyInfo);
            }
        }

        return _data;
    }

    //플레이어 현재 스텟데이터를 입력합니다. (0)
    private SaveData InputPlayerStat(SaveData _data)
    {
        if (playerAbilityManager == null)
        {
            Debug.LogWarning("playerAbilityManager인스턴스가 존재하지 않습니다.");
            return _data;
        }

        IReadOnlyDictionary<PlayerStat, object> ability = playerAbilityManager.PlayerAbilities;
        _data.playerLevel = (int)ability[PlayerStat.LEVEL];
        _data.playerExp = playerAbilityManager.GetCurrentExp;
        _data.playerHp = (int)ability[PlayerStat.HP];
        _data.playerMp = (int)ability[PlayerStat.MP];

        return _data;
    }

    //플레이어가 현재 보유한 골드,스킬포인트를 입력합니다.
    private SaveData InputMoneyAndSkillPoint(SaveData _data)
    {
        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager인스턴스가 존재하지 않습니다.");
            return _data;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("skillManager인스턴스가 존재하지 않습니다.");
            return _data;
        }

        _data.goldAmount = itemManager.GoldAmount;
        _data.skillPoint = skillManager.GetSkillPoint;

        return _data;
    }

    //인 게임 전역데이터의 값을 입력합니다.
    private SaveData InputGlobalData(SaveData _data)
    {
        _data.curNoOverlapItemKey = ItemManager.CurNoOverlapKey;
        _data.isSeePrologue = GlobalData.isSeePrologue;
        _data.loadSceneType = (int)GlobalData.curScene;
        _data.mapCode = GlobalData.curMapCode;

        return _data;
    }

    private SaveData InputQuestSaveData(SaveData _data)
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem != null)
        {
            _data.questSaveData = questSystem.CreateSaveData();
        }
        else
        {
            Debug.LogWarning("QuestSystem인스턴스가 존재하지 않습니다.");
        }

        return _data;
    }

    #endregion

    #region 데이터 로드 처리 과정 메소드
    //보유하고있는 아이템을 로드합니다.    (1)
    private void ApplyPossessedItemData(SaveData _data)
    {
        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager클래스가 존재하지 않습니다.");
            return;
        }

        foreach (var itemData in _data.possessedItemDatas)
        {
            Item getItem = itemDataBase.ItemDatabase.FirstOrDefault(x => x.ItemNum == itemData.itemNum);
            if (getItem is EquipmentItem)
            {
                getItem = Instantiate(getItem);
                getItem.ItemKey = itemData.itemKey;
            }

            itemManager.GetItem(getItem, itemData.amount);
        }
    }

    //착용된 장비아이템 리스트를 이용하여 장비를 착용합니다.    (2)
    private void ApplyEquippedItemData(SaveData _data)
    {
        if (equipmentManager == null)
        {
            Debug.LogWarning("IEquipmentManager클래스가 존재하지 않습니다.");
            return;
        }

        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager클래스가 존재하지 않습니다.");
            return;
        }

        foreach (var equippedItemKey in _data.equipList)
        {
            Item getItem = itemManager.ItemStacks.FirstOrDefault(x => x.item.ItemKey == equippedItemKey)?.item;
            if (getItem is EquipmentItem)
            {
                var getEquipmentItem = getItem as EquipmentItem;
                equipmentManager.Equip(getEquipmentItem.EquipmentType, getEquipmentItem);
            }
        }
    }

    //보유하고있는 장비 아이템 강화목록을 이용해서 보유아이템을 강화합니다.    (3)
    private void ApplyEnchantListData(SaveData _data)
    {
        if (equipmentManager == null)
        {
            Debug.LogWarning("IEquipmentManager클래스가 존재하지 않습니다.");
            return;
        }

        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager클래스가 존재하지 않습니다.");
            return;
        }

        foreach (var enchantInfo in _data.equipmentEnchantList)
        {
            EnchantItem enchantItem = enchantItemDataBase.EnchantItemDatabase.FirstOrDefault(x => x.ItemNum == enchantInfo.enchantNum);
            EquipmentItem equipmentItem = itemManager.ItemStacks.FirstOrDefault(x => x.item.ItemKey == enchantInfo.equipmentKey)?.item as EquipmentItem;

            if (enchantItem != null && equipmentItem != null)
            {
                equipmentManager.Enchant(equipmentItem, enchantItem, false);
            }
            else
            {
                Debug.LogWarning($"{enchantInfo.equipmentKey} 혹은 해당 아이템의 강화목록중 어떤 재료를 불러오지 못했습니다.");
            }
        }
    }

    //단축키에 등록된 소비아이템 데이터를 적용합니다. (4)
    private void ApplyConsumableItemHotKeyData(SaveData _data)
    {
        if (itemHotKeyInfo == null)
        {
            Debug.LogWarning("ConsumableItemHotKeyInfo스크립터블 오브젝트를 지정하지 않았습니다.");
            return;
        }

        for (int i = 0; i < _data.hotKeyItemList.Count; i++)
        {
            string targetKey = _data.hotKeyItemList[i];
            ConsumableItem item = itemDataBase.ItemDatabase.FirstOrDefault(x => x.ItemKey == targetKey) as ConsumableItem;
            itemHotKeyInfo.SetItem(i, item);
        }
    }

    //현재 플레이어 스킬정보 데이터를 적용합니다. (5)
    private void ApplyPossessedSkillData(SaveData _data)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("skillManager인스턴스가 존재하지 않습니다.");
            return;
        }

        foreach (var skillData in _data.possessedSkillList)
        {
            skillManager.PossessSkill(skillData.skillId, skillData.skillLevel);
        }
    }

    //단축키에 등록된 스킬정보 데이터를 적용합니다. (6)
    private void ApplySkillHotKeyData(SaveData _data)
    {
        if (skillHotKeyInfo == null)
        {
            Debug.LogWarning("SkillHotKeyInfo스크립터블 오브젝트를 지정하지 않았습니다.");
            return;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("skillManager인스턴스가 존재하지 않습니다.");
            return;
        }

        for (int i = 0; i < _data.hotKeySkillList.Count; i++)
        {
            if (_data.hotKeySkillList[i] == null)
                continue;

            string id = _data.hotKeySkillList[i].skillId;
            int level = _data.hotKeySkillList[i].skillLevel;
            var skill = skillManager.GetSkill(id, level);
            skillHotKeyInfo.SetSkill(i, skill);
        }
    }

    //플레이어 현재 스텟데이터를 적용합니다. (0)
    private void ApplyPlayerStat(SaveData _data)
    {
        if (playerAbilityManager == null)
        {
            Debug.LogWarning("playerAbilityManager인스턴스가 존재하지 않습니다.");
            return;
        }

        playerAbilityManager.SetPlayerAbility(_data.playerLevel);
        playerAbilityManager.AddExp(_data.playerExp);
        playerAbilityManager.SetHp(_data.playerHp);
        playerAbilityManager.SetMp(_data.playerMp);
    }

    //플레이어가 현재 보유한 골드,스킬포인트 데이터를 인게임에 적용합니다.
    private void ApplyMoneyAndSkillPoint(SaveData _data)
    {
        if (itemManager == null)
        {
            Debug.LogWarning("IItemManager인스턴스가 존재하지 않습니다.");
            return;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("skillManager인스턴스가 존재하지 않습니다.");
            return;
        }

        itemManager.GetGold(_data.goldAmount);
        skillManager.SetSkillPoint = _data.skillPoint;
    }

    //인 게임 전역데이터의 값을 인게임에 적용합니다.
    private void ApplyGlobalData(SaveData _data)
    {
        ItemManager.CurNoOverlapKey = _data.curNoOverlapItemKey;
        GlobalData.isSeePrologue = _data.isSeePrologue;
        GlobalData.curScene = (LoadSceneType)_data.loadSceneType;
        GlobalData.curMapCode = _data.mapCode;
    }

    private void ApplyQuestSaveData(SaveData _data)
    {
        var questSystem = QuestSystem.Instance;

        if (questSystem != null)
        {
            questSystem.LoadData(_data.questSaveData);
        }
        else
        {
            Debug.LogWarning("QuestSystem인스턴스가 존재하지 않습니다.");
        }
    }
    #endregion
}

[System.Serializable]
public class SaveData
{
    public SaveData()
    {
        possessedItemDatas = new();
        equipList = new();
        equipmentEnchantList = new();
        hotKeyItemList = new();
        possessedSkillList = new();
        hotKeySkillList = new();
    }

    public List<PossessedItemData> possessedItemDatas;   //보유하고있는 아이템key와 갯수의 튜플 리스트

    public List<string> equipList;  //장비 장착 리스트 (string : 장비아이템 key)
    public List<EquipmentEnchantData> equipmentEnchantList; //장비 강화 리스트

    public List<string> hotKeyItemList; //단축키에 등록된 아이템 리스트 (string : 아이템 키 값)

    public List<PossessedSkillData> possessedSkillList; //보유중인 스킬 데이터 리스트
    public List<PossessedSkillData> hotKeySkillList;    //단축키에 등록된 스킬 리스트

    //플레이어상태
    public int playerLevel;
    public int playerExp;
    public int playerHp;
    public int playerMp;

    //골드상태
    public int goldAmount;

    //스킬 포인트 상태
    public int skillPoint;

    public int curNoOverlapItemKey;
    public bool isSeePrologue;
    public int loadSceneType;   //저장할때 enum => int / 로드할때 int => enum
    public int mapCode;

    //퀘스트
    public string questSaveData;
}

[System.Serializable]
public class PossessedItemData
{
    public PossessedItemData(string _itemKey, int _itemNum, int _amount)
    { 
        itemKey = _itemKey;
        itemNum = _itemNum;
        amount = _amount;
    }

    public string itemKey;
    public int itemNum;
    public int amount;
}

[System.Serializable]
public class EquipmentEnchantData
{
    public EquipmentEnchantData(string _equipmentItemKey, int _enchantNum)
    { 
        equipmentKey = _equipmentItemKey;
        enchantNum = _enchantNum;
    }

    public string equipmentKey;  //대상 장비아이템
    public int enchantNum;    //대상 장비아이템을 강화했던 강화아이템 번호
}

[System.Serializable]
public class PossessedSkillData
{
    public PossessedSkillData(string _skillId, int _skillLevel)
    { 
        skillId = _skillId;
        skillLevel = _skillLevel;
    }

    public string skillId;
    public int skillLevel;
}
