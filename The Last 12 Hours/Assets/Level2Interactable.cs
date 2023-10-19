using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Interactable : Interactable
{
    // TO-DO: add level serializable field

    public override void Interact()
    {
        GameManager.LoadLevelScene(2);
    }
}
