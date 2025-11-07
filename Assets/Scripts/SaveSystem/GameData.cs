using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory; // itemSaveId -> stackSize
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;

    public SerializableDictionary<string, ItemType> equipedItems; // itemSaveId -> slotType

    public int skillPoints;
    public SerializableDictionary<string, bool> skillTreeUI; // Skill name -> unlock status
    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades; // Skill type -> upgradeType

    public SerializableDictionary<string, bool> unlockedCheckpoints; // checkpoint id -> unlocked status
    public SerializableDictionary<string, Vector3> inSceneTeleports; // scene name -> teleport (veil nexus) position

    public string teleportDestinationSceneName;
    public bool returningFromMarket;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equipedItems = new SerializableDictionary<string, ItemType>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();

        unlockedCheckpoints = new SerializableDictionary<string, bool>();
        inSceneTeleports = new SerializableDictionary<string, Vector3>();
    }
}
