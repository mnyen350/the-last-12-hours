using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<(float, Action)> _callbacks = new List<(float, Action)>();

    // Field to get the currentScene to avoid code redundancy.
    public Scene CurrentScene { get { return SceneManager.GetActiveScene(); } }

    [SerializeField]
    private Sound[] backgroundMusic;

    // Globally accessible mouse position, so we don't have to get it every time.
    [DoNotSerialize]
    public MouseProperties MouseProperties;

    public OutlineInteract OutlineInteract;

    public ItemSprites ItemSprites;
    public Prefabs Prefabs;
    public new Camera camera { get; private set; }

    public void DelayCallback(TimeSpan time, Action action)
    {
        _callbacks.Add((Time.time + (float)time.TotalSeconds, action));
    }

    private void Awake()
    {
        // This is for setting the instance of the singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        Sound.InitializeSounds(gameObject, backgroundMusic);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        float now = Time.time;
        for (int i = 0; i < _callbacks.Count; i++)
        {
            if (now >= _callbacks[i].Item1)
            {
                _callbacks[i].Item2.Invoke();
                _callbacks.RemoveAt(i);
                i--; // resync i
            }
        }

        // Gets the mouse useful properties.
        MouseProperties = GetMouseProperties();
    }

    private MouseProperties GetMouseProperties()
    {
        if (!camera) return null;
        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - camera.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return new MouseProperties
        {
            MouseScreenPosition = mousePosition,
            MouseDirection = direction,
            Angle = angle
        };
    }

    // An event when a scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded name={scene.name}, mode={mode}");

        // Plays and stops the background music acording to the scene name.
        foreach (Sound sound in backgroundMusic)
        {
            if (sound.Name == CurrentScene.name)
            {
                sound.Source.Play();
            }
            else
            {
                sound.Source.Stop();
            }
        }

        camera = GameObject.Find("Camera")?.GetComponentInChildren<Camera>();
        if (camera == null)
            Debug.LogWarning("Camera not found!");


        // for chapter scenes, load the ui scenes
        if (scene.name.StartsWith("Chapter", StringComparison.OrdinalIgnoreCase))
        {
#warning TO-DO: do this a better way, but for now this suffices for being able to start the game from any chapter scene
            int level = int.Parse(scene.name.Substring(7));
            Player.Instance.EnterLevel(level);

            SceneManager.LoadScene("HealthUI", LoadSceneMode.Additive);
            SceneManager.LoadScene("InventoryMenu", LoadSceneMode.Additive);
        }
    }

    public static void LoadMainMenuScene() => SceneManager.LoadScene(0);
    public static void LoadSettingsScene() => SceneManager.LoadScene("SettingsMenu");
    public static void LoadLevelScene(int level)
    {
        /*UnityAction<Scene, LoadSceneMode> enterLevelCallback = null;
        enterLevelCallback = (a,b) =>
        {
            Player.Instance.EnterLevel(level);
            // now unload this event
            SceneManager.sceneLoaded -= enterLevelCallback;
        };

        SceneManager.sceneLoaded += enterLevelCallback;*/
        SceneManager.LoadScene($"Chapter{level}");

    }
    public static void LoadGameOverScene() => SceneManager.LoadScene("GameOverMenu");
}

[Serializable]
public class OutlineInteract
{
    public float Distance = 2;

    public Material Outline;
    public Material NoOutline;
}

[Serializable]
public class ItemSprites
{
    public Sprite Flashlight;
    public Sprite Knife;
    public Sprite Gun;
    public Sprite Axe;
    public Sprite Bandage;
    public Sprite Ammo;
}

[Serializable]
public class Prefabs
{
    public GameObject RatPrefab;

    public GameObject BandagePrefab;
}

public class MouseProperties
{
    public Vector3 MouseScreenPosition = new Vector3();
    public Vector3 MouseDirection = new Vector3();
    public float Angle = 0;
}