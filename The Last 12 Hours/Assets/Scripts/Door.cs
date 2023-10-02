using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool closed = true;

    [SerializeField]
    private Sprite closedDoor;

    [SerializeField]
    private Sprite openedDoor;

    private SpriteRenderer sr;

    private BoxCollider2D collider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    public override void Interact()
    {
        if (closed)
        {
            sr.sprite = openedDoor;
        }
        else
        {
            sr.sprite = closedDoor;
        }

        collider.isTrigger = closed;
        closed = !closed;
    }
}
