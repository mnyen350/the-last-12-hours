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

    [SerializeField]
    public Controls PlayerControls;

    // Hand of the player and components
    private GameObject hand;
    private Light2D flashlight;

    [SerializeField]
    public float interactDistance = 5;

    [SerializeField]
    private Sound[] sounds;

    public Inventory inventory;
    private bool IsInventoryOpen; 
    public int level;

    public Vector2 position => this.rb.position;

    public void Reset()
    {
        health = maxHealth;
        IsInventoryOpen = false;
    }

    protected override void Awake()
    {
        maxHealth = 10;
        Reset();


        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        hand = transform.Find("Hand").gameObject;
        flashlight = hand.GetComponentInChildren<Light2D>();

        OnStartMoving += () => PlaySound("Walking");
        OnStopMoving += () => StopSound("Walking");
        OnDeath += () => GameManager.LoadGameOverScene();

        Sound.InitializeSounds(gameObject, sounds);

        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);

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
            if (!IsInventoryOpen)
            {
                IsInventoryOpen = true;
                SceneManager.LoadScene("InventoryMenu", LoadSceneMode.Additive);
            }
            else
            {
                IsInventoryOpen = false;
                SceneManager.UnloadSceneAsync("InventoryMenu");
            }
        }

        // Interact with objects
        if (Input.GetKeyDown(PlayerControls.Interact))
        {
            Interact();
        }
        // Turn on/off the flashlight
        if (Input.GetKeyDown(PlayerControls.Flashlight))
        {
            TurnFlashLight();
        }

        // Mouse left click for fire.
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void UpdateMove()
    {
        Vector2 movement = PlayerControls.GetMovement();
        this.ApplyMovement(movement);
    }

    void FixedUpdate()
    {
        // Using FixedUpdate to get a Physics acurate movement.
        UpdateMove();
    }
    private void Interact()
    {
        // Gets the list of interactables and then gets the first one if it's not null.
        var interactables = Physics2D.OverlapCircleAll(transform.position, interactDistance)
            //.Where(x => x.CompareTag("Interactable"))
            .Select(x => x.GetComponent<Interactable>())
            .Where(x => x != null)
            .OrderBy(x => Vector2.Distance(x.transform.position, transform.position));

        interactables.FirstOrDefault()?.Interact();
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
        if (level == 1)
            Reset();

        // move to spawn point of level
        var spawn = GameObject.Find("Spawnpoint");
        if (spawn != null)
            transform.position = spawn.transform.position;
    }

    protected override void Attack()
    {
        Debug.Log("Attack Player");
    }

    protected override void Walk()
    {
        Debug.Log("Walk Player");
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

    // This is the use/fire/consume button
    private void Fire()
    {
        
    }

    // Enables and disables the flashlight depending of the state of this.
    private void TurnFlashLight()
    {
        PlaySound("Turn-On");
        flashlight.enabled = !flashlight.enabled;
    }

    // Plays the sound based on the name, if doesn't match won't be played
    private void PlaySound(string soundName)
    {
        Sound sound = Array.Find(sounds, x => x.Name == soundName);
        if (sound == null || sound.Source.isPlaying) return;
        sound.Source.Play();
    }

    // Stops the sound based on the name, if doesn't match won't be stopped
    private void StopSound(string soundName)
    {
        Sound sound = Array.Find(sounds, x => x.Name == soundName);
        if (sound == null || !sound.Source.isPlaying) return;
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