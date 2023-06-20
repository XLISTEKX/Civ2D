using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjects : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Tile _))
        {
            collision.GetComponent<SpriteRenderer>().enabled = true;

            if(collision.TryGetComponent(out Animator animator))
            {
                animator.enabled = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Tile _))
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
            if (collision.TryGetComponent(out Animator animator))
            {
                animator.enabled = false;
            }
        }
    }
}
