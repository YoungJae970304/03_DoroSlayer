using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    ShootTrap shootTrap;
    float spd = 5f;

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
        transform.Translate(Vector2.right * Time.deltaTime * spd);
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
