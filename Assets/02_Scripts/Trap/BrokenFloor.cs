using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFloor : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxCol;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isBroken");
        }
    }
    
    public void EventSetColliderOff()
    {
        boxCol.enabled = false;
        gameObject.SetActive(false);
    }
}
