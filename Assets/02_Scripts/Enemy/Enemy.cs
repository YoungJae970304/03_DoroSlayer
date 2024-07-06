using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float atk = 25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(atk);
        }
    }
}
