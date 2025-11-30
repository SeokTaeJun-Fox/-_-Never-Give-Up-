using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 스텟 종류
public enum PlayerStat
{ 
    HP = 0,
    MP,
    MAXHP,
    MAXMP,
    ATK,
    DEF,
    ADD_MAXHP,
    ADD_MAXMP,
    ADD_ATK,
    ADD_DEF,
    TOTAL_ATK,
    TOTAL_DEF,
    TOTAL_MAXHP,
    TOTAL_MAXMP,
    ATTACKRANGE,
    LEVEL,
    EXP
}

[Flags]
//아이템 종류
public enum ItemType
{ 
    None = 0,
    CONSUMABLE = 1 << 0,
    ENCHANT = 1 << 1,
    NORMAL = 1 << 2,
    EQUIPMENT = 1 << 3
}

public enum EquipmentType
{ 
    NONE = 0,
    WEAPON,
    HELMET,
    ARMOR,
    RING
}

//아이템 리스트 패널의 토글탭에 사용되는 아이템 종류
public enum ItemListType
{ 
    TOTAL,
    CONSUMABLE,
    ENCHANT,
    NORMAL,
    EQUIPMENT,
}

public enum CanvasType  //캔버스 종류입니다. (캔버스마다 다른 레이어값을 가지고있습니다.)
{
    None = -10,
    WorldLayer = -1,
    MainLayer = 0,
    DialogLayer = 1,
    FullScreenLayer = 2,
    AlwaysTopLayer = 3,
}

public enum UIType
{ 
    INVENTORY,
    SKILLWINDOW,
    CHARACTERSTATWINDOW,
    CONSUMABLE_ITEM_SLOT_WINDOW,
    SKILL_SLOT_WINDOW,
    PLAYER_ABILITY_WINDOW,
    LOADING_WINDOW,
    DIALOG_WINDOW,
    SHOP_WINDOW,
    SELECTED_BUY_ITEM_PANEL,
    SELECTED_SELL_ITEM_PANEL,
    POPUP_WINDOW,
    QUEST_LIST_WINDOW,
    REWARD_WINDOW,
    QUEST_WINDOW,
    PORTAL_WINDOW,
    ENDING_WINDOW,
    MENU_WINDOW,
    SETTING_WINDOW
}

public enum EquipmentState
{ 
    UNEQUIPPED,    //착용되지 않은 장비
    EQUIPPED,      //착용된 장비
    NOPOSSESSION,    //소유하고 있지 않은 장비
    SAME_WEAPON_SELECTED  //같은 무기를 선택한 상태
}

public enum LoadSceneType
{ 
    VILLAGE_SCENE,
    HUNTING_GROUND_SCENE,
    BOSS_SCENE,
    LOBBY_SCENE
}

//NPC
public enum NpcQuestState   //NPC 퀘스트 상태
{
    NONE,   //퀘스트 요청 없음 상태
    QUEST_READY,    //퀘스트목록 1개 이상 존재할시 퀘스트 대기상태
    QUEST_IN_PROGRESS,  //퀘스트 진행중
    QUEST_COMPLETE_READY,   //퀘스트 완료 대기상태
}