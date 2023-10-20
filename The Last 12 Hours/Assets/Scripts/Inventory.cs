using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Inventory : IEnumerable<Item>
{
    private List<Item> _items;

    public Inventory()
    {
        _items = new List<Item>();
    }

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    public IEnumerator<Item> GetEnumerator() => _items.GetEnumerator();
    public Item Get(ItemType type) => this.FirstOrDefault(item => item.type == type);

    public event Action<Item> OnAddItem;
    public event Action<Item> OnRemoveItem;
    public event Action<Item> OnUpdateItem;
    public event Action OnChange;

    // Add items to the list
    public void Add(Item item)
    {
        var existing = Get(item.type);

        if (existing != null)
        {
            existing.amount += item.amount;
            OnUpdateItem?.Invoke(existing);
        }
        else
        {
            _items.Add(item);
            OnAddItem?.Invoke(item);
        }

        OnChange?.Invoke();
    }

    public void Remove(ItemType type, int amount = 1)
    {
        var existing = Get(type);
        if (existing == null)
            return;

        if (existing.amount > amount)
        {
            existing.amount -= amount;
            OnUpdateItem?.Invoke(existing);
        }
        else
        {
            _items.Remove(existing);
            OnRemoveItem?.Invoke(existing);
        }

        OnChange?.Invoke();
    }
}