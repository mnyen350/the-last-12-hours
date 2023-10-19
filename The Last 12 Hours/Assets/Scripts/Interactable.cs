using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    protected new BoxCollider2D collider = null;
    protected bool interacted = false;

    public bool outline = true;

    public AudioSource InteractSound; 
    public bool ChangeColliderState = true;

    protected virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Interact()
    {
        interacted = !interacted;
        if (collider && ChangeColliderState)
            collider.isTrigger = interacted;
    }

    // Gets all the components type SpriteRenderer to the array
    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (outline) CheckOutline();
    }

    // Constantly check if a player is near the interactable.
    private void CheckOutline()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.Instance.transform.position, Player.Instance.interactDistance);

        if (colliders.Any(x => x.CompareTag("Interactable") && x.gameObject == this.gameObject))
        {
            SetOutline(GameManager.Instance.OutlineInteract.Outline);
        }
        else
        {
            SetOutline(GameManager.Instance.OutlineInteract.NoOutline);
        }
    }

    // This sets the outline of interactable.
    private void SetOutline(Material outlineMaterial)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = outlineMaterial;
        }
    }

    public void PlayInteractSound()
    {
        InteractSound?.Play();
    }
}
