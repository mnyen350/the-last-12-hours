using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player : Entity
{ 
    // Static reference to the instance
    public static Player Instance { get; private set; }

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private static readonly int[] NO_PLAYER_SCENES = new int[] { 0 };

    [SerializeField]
    private float interactDistance = 5;

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
        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Do not overload the main method
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
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
        List<Interactable> interactables = Physics2D.OverlapCircleAll(transform.position, interactDistance).Where(x => x.CompareTag("Interactable")).Select(x => x.GetComponent<Interactable>()).ToList();
        foreach(Interactable interactable in interactables)
        {
            Debug.Log("test");
            interactable.Interact();
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
}
