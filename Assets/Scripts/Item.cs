using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Etc
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
}


