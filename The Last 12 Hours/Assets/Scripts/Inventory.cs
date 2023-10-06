using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Inventory
{
    public bool CanOpenInventory { get; set; }

    // Hotbar array for fixed space for fast accessible items
    [DoNotSerialize]
    public Item[] Hotbar = new Item[3];

    // Items list for storing all the items
    public List<Item> Items = new List<Item>();

    // Add items to the list
    public void AddItems(Item item)
    {
        if (IsValidItem(item, out Item existItem))
        {
            existItem.Amount += item.Amount;
        }
        else
        {
            Items.Add(item);
        }
    }

    // Set items to the hotbar (might cause an issue because of index)
    public void SetItemHotBar(Item item, int index)
    {
        Hotbar[index] = item;
    }

    // Use the hotbar item, first checking it and then passing it to UseItem
    public void UseHotBarItem(int index)
    {
        if (Hotbar[index] is Item item && item != null)
        {
            UseItem(item);
        }
    }

    // Use item from the items list and handle the amounts.
    public void UseItem(Item item)
    {
        if (!IsValidItem(item, out Item existItem)) return;

        existItem.Use();

        if (item.IsEquipment) return;

        existItem.Amount--;

        if (item.Amount <= 0)
        {
            int indexInHotbar = Array.FindIndex(Hotbar, x => x == existItem);
            if (indexInHotbar != -1)
            {
                Hotbar[indexInHotbar] = null;
            }

            Items.Remove(existItem);
        }
    }

    // Validates if the item exists in the inventory
    private bool IsValidItem(Item item, out Item existItem)
    {
        existItem = Items.FirstOrDefault(x => x.Name == item.Name);
        return existItem != null;
    }
}