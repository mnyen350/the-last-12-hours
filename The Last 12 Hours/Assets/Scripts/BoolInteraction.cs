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

    public bool ChangeColliderState = true;
    private new BoxCollider2D collider = null;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public override void Interact()
    {
        firstState.SetActive(interacted);
        secondState.SetActive(!interacted);

        interacted = !interacted;
        if (collider && ChangeColliderState) collider.isTrigger = interacted;
    }

}
