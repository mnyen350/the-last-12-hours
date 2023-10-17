using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Field to get the currentScene to avoid code redundancy.
    public Scene CurrentScene { get { return SceneManager.GetActiveScene(); } }

    [SerializeField]
    private Sound[] backgroundMusic;

    // Globally accessible mouse position, so we don't have to get it every time.
    [DoNotSerialize]
    public MouseProperties MouseProperties;

    public OutlineInteract OutlineInteract;


    private new Camera camera;

    public static GameManager Instance { get; private set; }

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
        // Remember to not overload this.

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

    // Moves the player to the spawnpoint
    public void SpawnPlayer()
    {
        if (GameObject.Find("Spawnpoint") is GameObject spawn)
        {
            Player.Instance.transform.position = spawn.transform.position;
        }
    }

    // An event when a scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();
        Player.Instance.UpdateScene();

        // Plays and stops the background music acording to the scene name.
        foreach(Sound sound in backgroundMusic)
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

        camera = GameObject.Find("Camera").GetComponentInChildren<Camera>();
    }


    // Moves the player to the next scene and then spawns the player
    public static void NextScene()
    {
        SceneManager.LoadScene(Instance.CurrentScene.buildIndex + 1);
    }
}

[Serializable]
public class OutlineInteract
{
    public float Distance = 2;

    public Material Outline;
    public Material NoOutline;
}

public class MouseProperties
{
    public Vector3 MouseScreenPosition = new Vector3();
    public Vector3 MouseDirection = new Vector3();
    public float Angle = 0;
}