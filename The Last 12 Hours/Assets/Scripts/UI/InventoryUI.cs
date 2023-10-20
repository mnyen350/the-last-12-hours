using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    public static bool Enabled
    {
        get => Instance._canvas.enabled;
        set => Instance._canvas.enabled = value;
    }

    private Canvas _canvas;

    public Player player => Player.Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _canvas = this.GetComponent<Canvas>();
        _canvas.enabled = false;

        player.inventory.OnChange += Inventory_OnChange;
    }

    private void Inventory_OnChange()
    {
        UpdateItems("Equipment", i => i.isEquipment);
        UpdateItems("InventorySlot", i => !i.isEquipment);
    }

    private void UpdateItems(string tag, Func<Item, bool> predicate)
    {
        var slots = GameObject
            .FindGameObjectsWithTag(tag)
            .OrderBy(x => x.name)
            .Select(x => x.GetComponent<Image>())
            .ToArray();

        var items = player.inventory
            .Where(i => predicate(i))
            .ToArray();

        // limited by slots length
        for (int i = 0; i < slots.Length; i++)
        {
            var cws = slots[i].GetComponent<InventoryItemSlot>();
            var img = slots[i].GetComponent<Image>();

            if (i < items.Length)
            {
                cws.itemType = items[i].type;
                img.sprite = items[i].sprite;
                img.enabled = true;
            }
            else
            {
                img.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
