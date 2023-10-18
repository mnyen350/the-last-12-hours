using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolInteraction : Interactable
{
    private bool interacted = false;

    public bool oneInteraction = false;

    [SerializeField]
    private GameObject firstState;

    [SerializeField]
    private GameObject secondState;

    public bool ChangeColliderState = true;
    private new BoxCollider2D collider = null;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Interacts with it and changes the state of the object.
    public override void Interact()
    {
        if (interacted && oneInteraction) return;

        firstState.SetActive(interacted);
        secondState.SetActive(!interacted);

        interacted = !interacted;
        if (collider && ChangeColliderState) collider.isTrigger = interacted;

        PlayInteractSound();
    }

}
