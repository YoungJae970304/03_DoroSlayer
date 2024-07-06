using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int atk = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(atk);
        }
    }
}
