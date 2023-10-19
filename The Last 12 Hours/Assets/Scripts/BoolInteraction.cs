using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolInteraction : Interactable
{
    private bool interacted = false;

    [SerializeField]
    private GameObject firstState;

    [SerializeField]
    private GameObject secondState;

    // Interacts with it and changes the state of the object.
    public override void Interact()
    {
        firstState.SetActive(interacted);
        secondState.SetActive(!interacted);

        PlayInteractSound();

        base.Interact();
    }

}
