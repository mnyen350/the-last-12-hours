using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool closed = true;

    [SerializeField]
    private GameObject closedDoor;

    [SerializeField]
    private GameObject openedDoor;

    private new BoxCollider2D collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public override void Interact()
    {
        openedDoor.SetActive(closed);
        closedDoor.SetActive(!closed);

        collider.isTrigger = closed;
        closed = !closed;
    }

}
