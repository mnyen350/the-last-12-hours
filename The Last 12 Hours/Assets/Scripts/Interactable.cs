using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    public bool Outline = true;
    public abstract void Interact();

    // Gets all the components type SpriteRenderer to the array
    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Outline) CheckOutline();
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
}
