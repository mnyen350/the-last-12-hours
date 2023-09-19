using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerSceneIndex = 0;

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

        playerSceneIndex = SceneManager.GetActiveScene().buildIndex;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
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
    }


    // Moves the player to the next scene and then spawns the player
    public static void NextScene()
    {
        Instance.playerSceneIndex++;
        SceneManager.LoadScene(Instance.playerSceneIndex);
    }
}
