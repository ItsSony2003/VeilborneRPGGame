using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
    }
}
