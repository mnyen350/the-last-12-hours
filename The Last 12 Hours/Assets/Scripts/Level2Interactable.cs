using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Interactable : Interactable
{
    // TO-DO: add level serializable field

    void Start()
    {
        this.OnInteractingChange += () =>
        {
            if (this.isInteracting)
            {
                GameManager.LoadLevelScene(2);
            }
        };
    }
}
