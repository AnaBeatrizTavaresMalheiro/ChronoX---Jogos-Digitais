using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInTrash : MonoBehaviour
{
    private bool isNearTrash = false;
    private bool isHidden = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isNearTrash && Input.GetKeyDown(KeyCode.E))
        {
            isHidden = !isHidden;
            spriteRenderer.enabled = !isHidden;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TrashCan"))
        {
            isNearTrash = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TrashCan"))
        {
            isNearTrash = false;

            // Se sair da área, reaparece automaticamente
            if (isHidden)
            {
                isHidden = false;
                spriteRenderer.enabled = true;
            }
        }
    }
}
