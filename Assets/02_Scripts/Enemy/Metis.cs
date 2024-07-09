using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Metis : MonoBehaviour
{
    float spd = 2f;

    Vector3 playerPos;
    Vector2 dir;

    SpriteRenderer sr;

    GameObject player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        dir = (playerPos - transform.position).normalized;

        StartCoroutine(CutAcitve());
    }

    IEnumerator CutAcitve()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        sr.flipX = dir.x > 0;
        transform.Translate(new Vector2(dir.x * Time.deltaTime * spd, 0));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            StartCoroutine(damageDelay());
        }
    }
    IEnumerator damageDelay()
    {
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<Player>().TakeDamage(1);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
