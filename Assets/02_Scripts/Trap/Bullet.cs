using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    ShootTrap shootTrap;

    private void Awake()
    {
        shootTrap = transform.GetComponentInParent<ShootTrap>();
    }

    private void OnEnable()
    {
        StartCoroutine(SetFalse());
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(1);
            shootTrap.bullets.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(2.5f);
        shootTrap.bullets.Add(gameObject);
        gameObject.SetActive(false);
    }
}
