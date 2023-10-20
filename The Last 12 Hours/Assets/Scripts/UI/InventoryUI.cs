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
        UpdateEquipment();
    }

    private void UpdateEquipment()
    {
        var equipmentSlots = GameObject
            .FindGameObjectsWithTag("Equipment")
            .OrderBy(x => x.name)
            .Select(x => x.GetComponent<Image>())
            .ToArray();

        var equipment = player.inventory
            .Where(i => i.isEquipment)
            .ToArray();

        // should never happen
        if (equipment.Length > equipmentSlots.Length)
        {
            Debug.LogWarning("More equipment in inventory than equipment slots in UI");
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            var cws = equipmentSlots[i].GetComponent<ClickWeapon>();
            var img = equipmentSlots[i].GetComponent<Image>();

            if (i < equipment.Length)
            {
                cws.weaponType = equipment[i].type;
                img.sprite = equipment[i].sprite;
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
