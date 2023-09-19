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
    private static readonly int[] NO_PLAYER_SCENES = new int[] { 0 };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sr = GetComponent<SpriteRenderer>();
        // This is for not destroying the player when the scene is changed
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Do not overload the main method
        UpdateMove();
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
        Vector3 movement = new Vector3(x, y, 0);
        transform.Translate(movement * Speed * Time.deltaTime);
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
