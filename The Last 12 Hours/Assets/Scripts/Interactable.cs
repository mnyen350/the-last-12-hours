using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    public bool Outline = true;
    public abstract void Interact();

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Outline) CheckOutline();
    }

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

    private void SetOutline(Material outlineMaterial)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material = outlineMaterial;
        }
    }
}
