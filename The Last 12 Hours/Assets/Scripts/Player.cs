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
using static UnityEditor.Progress;

[System.Serializable]
public class Player : Entity
{
    // Static reference to the instance
    public static Player Instance { get; private set; }

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Hand of the player and components
    private GameObject hand;
    private Light2D flashlight;

    private static readonly int[] NO_PLAYER_SCENES = new int[] { 0 };

    public Inventory Inventory;

    [SerializeField]
    public float interactDistance = 5;

    private Animator animator;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hand = transform.Find("Hand").gameObject;
        flashlight = hand.GetComponentInChildren<Light2D>();
        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Do not overload the main method

        // KeyCode for Input
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // Updates the hand position and handles different things about the hand
        UpdateHand();
    }

    // For Updating player acording to the scene.
    public void UpdateScene()
    {
        bool isNoPlayerScene = NO_PLAYER_SCENES.Contains(GameManager.Instance.playerSceneIndex);

        // Could use !isNoPlayerScene, but this looks better for scaling code.
        sr.enabled = isNoPlayerScene ? false : true;
    }
    private void UpdateMove()
    {
        if (canMove == false) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Using velocity so it doesn't get buggy on the walls
        Vector2 movement = new Vector2(x, y);
        rb.velocity = movement * Speed;

        animator.SetFloat("speed", movement.magnitude);

        if (x > 0)
        {
            sr.flipX = false;
        }
        else if (x < 0)
        {
            sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        // Using FixedUpdate to get a Physics acurate movement.
        UpdateMove();
    }
    private void Interact()
    {
        if (canMove == false) return;

        // Gets the list of interactables and then gets the first one if it's not null.
        List<Interactable> interactables = Physics2D.OverlapCircleAll(transform.position, interactDistance).Where(x => x.CompareTag("Interactable")).Select(x => x.GetComponent<Interactable>()).OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).ToList();
        if (interactables.FirstOrDefault() is Interactable interactable)
        {
            // Interacts with the object.
            interactable.Interact();
        }
    }

    private void UpdateHand()
    {
        // Updates the hand rotation/position
        if (GameManager.Instance.MouseProperties is MouseProperties mouseProperties)
        {
            hand.transform.rotation = Quaternion.AngleAxis(mouseProperties.Angle, Vector3.forward);
        }
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
        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        Debug.Log("Player healed");
    }

    // This is the use/fire/consume button
    private void Fire()
    {
        TurnFlashLight();
    }

    // Enables and disables the flashlight depending of the state of this.
    private void TurnFlashLight()
    {
        flashlight.enabled = !flashlight.enabled;
    }
}