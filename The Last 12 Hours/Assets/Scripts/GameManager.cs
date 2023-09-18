using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int playerSceneIndex = 0;

    // Public player object so we don't need to find it by code.
    public GameObject playerObject;

    public static GameManager Instance;
    
    void Awake()
    {
        // This is for setting the instance of the singleton
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        playerObject = GameObject.Find("Player");
    }

    // Moves the player to the spawnpoint
    public void SpawnPlayer()
    {
        GameObject spawn = GameObject.Find("Spawnpoint");

        if(spawn != null)
        {
            Debug.LogError("Spawnpoint is not set");
        }
        else
        {
            playerObject.transform.position = spawn.transform.position;
        }
    }


    // Moves the player to the next scene and then spawns the player
    public void NextScene()
    {
        playerSceneIndex++;
        SceneManager.LoadScene(playerSceneIndex);

        SpawnPlayer();
    }
}
