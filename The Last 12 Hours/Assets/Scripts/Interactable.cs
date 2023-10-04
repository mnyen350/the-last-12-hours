using System.Collections;
using System.Collections.Generic;
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
        if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= GameManager.Instance.OutlineInteract.Distance)
        {
             foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.material = GameManager.Instance.OutlineInteract.Outline;
            }
        }
        else
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.material = GameManager.Instance.OutlineInteract.NoOutline;
            }
        }
    }
}
