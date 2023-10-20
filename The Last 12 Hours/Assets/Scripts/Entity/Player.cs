using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player : Entity
{
    // Static reference to the instance
    public static Player Instance { get; private set; }
    private static Action _afterInstanceCallback;

    public static void AfterInstance(Action action)
    {
        if (Instance != null)
            action();
        else
            _afterInstanceCallback += action;
    }

    [SerializeField]
    public Controls PlayerControls;

    [SerializeField]
    public float interactDistance = 5;

    [SerializeField]
    private Sound[] sounds;

    public GameObject hand { get; private set; }
    public override int attack => 1;
    public ItemType activeWeapon { get; private set; }
    public int level { get; private set; }
    public Inventory inventory { get; private set; }

    public bool flashlight
    {
        get => GetComponentInChildren<Light2D>()?.enabled ?? false;
        set
        {
            var light = GetComponentInChildren<Light2D>();
            if (light)
                light.enabled = value;
        }
    }

    public event Action OnChangeWeapon;

    public void ChangeWeapon(ItemType type)
    {
        activeWeapon = type;

        var sr = player.hand.GetComponentInChildren<SpriteRenderer>();
        if (type == ItemType.Undefined)
        {
            sr.enabled = false;
        }
        else
        {            
            // disable flashlight if weapon isn't flashlight
            if (type != ItemType.Flashlight)
                flashlight = false;

            sr.sprite = Item.GetSprite(type);
            sr.enabled = true;
        }

        OnChangeWeapon?.Invoke();
    }

    public void Reset()
    {
        health = maxHealth;
    }

    protected override void Awake()
    {
        Debug.Log("Player Awake()");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        hand = transform.Find("Hand").gameObject;
        inventory = new Inventory();

        OnStartMoving += () => PlaySound("Walking");
        OnStopMoving += () => StopSound("Walking");
        OnDeath += () => GameManager.LoadGameOverScene();

        ChangeWeapon(ItemType.Undefined);
        Reset();

        Sound.InitializeSounds(gameObject, sounds);

        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);

        _afterInstanceCallback?.Invoke();
        _afterInstanceCallback = null;

        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        // Updates for handling input operations.
        UpdateInput();
        // Updates the hand position and handles different things about the hand
        UpdateHand();
    }


    public void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryUI.Enabled = !InventoryUI.Enabled;
        }

        // Interact with objects
        if (Input.GetKeyDown(PlayerControls.Interact))
        {
            // Gets the list of interactables and then gets the first one if it's not null.
            var interactable = GetNearby<Interactable>(interactDistance).FirstOrDefault();
            interactable?.Interact();
        }

        // Turn on/off the flashlight
        if (Input.GetKeyDown(PlayerControls.Flashlight))
        {
            if (this.activeWeapon != ItemType.Undefined)
            {
                var weapon = this.inventory.Get(this.activeWeapon);
                weapon?.UseWeaponAttack();
            }
        }
    }

    void FixedUpdate()
    {
        // Using FixedUpdate to get a Physics acurate movement.
        var movement = PlayerControls.GetMovement();
        this.ApplyMovement(movement);
    }

    private void UpdateHand()
    {
        // Updates the hand rotation/position
        if (GameManager.Instance.MouseProperties is MouseProperties mouseProperties)
        {
            hand.transform.rotation = Quaternion.AngleAxis(mouseProperties.Angle, Vector3.forward);
        }
    }

    public void EnterLevel(int level)
    {
        this.level = level;
        if (level == 1)
            Reset();

        // move to spawn point of level
        var spawn = GameObject.Find("Spawnpoint");
        if (spawn != null)
            transform.position = spawn.transform.position;
    }

    protected void Heal(int healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);
        Debug.Log("Player healed");
    }

    public override void ReceiveAttack(Entity source, int damage)
    {
        base.ReceiveAttack(source, damage);
        Debug.Log($"player received damage, hp: {health} / {maxHealth}");
    }

    // Plays the sound based on the name, if doesn't match won't be played
    private void PlaySound(string soundName)
    {
        var sound = sounds.FirstOrDefault(x => x.Name == soundName);
        if (sound?.Source?.isPlaying == true)
            sound.Source.Play();
    }

    // Stops the sound based on the name, if doesn't match won't be stopped
    private void StopSound(string soundName)
    {
        var sound = sounds.FirstOrDefault(x => x.Name == soundName);
        if (sound?.Source?.isPlaying == false)
            sound.Source.Stop();
    }
}

[Serializable]
public class Controls
{
    // Movement
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Right = KeyCode.D;
    public KeyCode Left = KeyCode.A;

    // Equipment
    public KeyCode HotbarFirst = KeyCode.Alpha1;
    public KeyCode HotbarSecond = KeyCode.Alpha2;
    public KeyCode HotbarThird = KeyCode.Alpha3;
    public KeyCode Inventory = KeyCode.Tab;

    // Flashlight
    public KeyCode Flashlight = KeyCode.F;

    // Use
    public KeyCode Reload = KeyCode.R;
    public KeyCode Heal = KeyCode.Q;
    public KeyCode Interact = KeyCode.E;

    public Vector2 GetMovement()
    {
        float vertical;
        float horizontal;

        // Vertical Movement
        if (Input.GetKey(Up))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(Down))
        {
            vertical = -1f;
        }
        else
        {
            vertical = 0f;
        }

        // Horizontal Movement
        if (Input.GetKey(Right))
        {
            horizontal = 1f;
        }
        else if (Input.GetKey(Left))
        {
            horizontal = -1f;
        }
        else
        {
            horizontal = 0f;
        }

        return new Vector2(horizontal, vertical);
    }
}