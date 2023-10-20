using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Undefined = 0,
    Ammo,
    Battery,
    Bandage, 
    Flashlight,
    Axe,
    Knife,
    Gun
}
public class Item : MonoBehaviour
{
    private static ItemType[] AUTO_CONSUME_TYPES = new[] { ItemType.Ammo };
    private static ItemType[] CONSUMABLES_TYPES = new[] { ItemType.Ammo, ItemType.Battery, ItemType.Bandage };
    private static ItemType[] EQUIPMENT_TYPES = new[] { ItemType.Flashlight, ItemType.Axe, ItemType.Knife, ItemType.Gun };

    public Player player => Player.Instance;
    public GameManager gameManager => GameManager.Instance;

    [field: SerializeField]
    public ItemType type { get; private set; }
    [field: SerializeField]
    public int amount { get; set; }

    public string itemName => type.ToString();

    public bool isEquipment => EQUIPMENT_TYPES.Contains(type);
    public bool IsConsumable => CONSUMABLES_TYPES.Contains(type);
    public bool isAutoConsume => AUTO_CONSUME_TYPES.Contains(type);

    public Sprite sprite
    {
        get
        {
            switch (type)
            {
                case ItemType.Axe: return gameManager.ItemSprites.Axe;
                case ItemType.Gun: return gameManager.ItemSprites.Axe;
                case ItemType.Knife: return gameManager.ItemSprites.Axe;
                case ItemType.Flashlight: return gameManager.ItemSprites.Flashlight;
            }
            return null;
        }
    }

    public void Start()
    {
        var sprite = this.sprite;
        if (sprite != null)
            GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void Pickup()
    {
        Debug.Log("Player is picking up " + itemName);

        // remove from world
        Destroy(this.gameObject);

        if (isAutoConsume)
        {
            Debug.Log($"Auto consuming item ${type}");
            switch (type)
            {
                case ItemType.Ammo:
                    break;
            }
        }
        else
        {
            player.inventory.Add(this);
        }
    }

    public void Use()
    {
        // Here will be the Use logic of the item.
    }
}
