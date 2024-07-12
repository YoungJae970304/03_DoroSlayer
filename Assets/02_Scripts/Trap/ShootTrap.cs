using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : MonoBehaviour
{
    public GameObject bullet;
    int bulletCount = 3;
    public List<GameObject> bullets = new List<GameObject>();

    void Start()
    {
        LoadBullet();
        StartCoroutine(Shoot());
    }

    void LoadBullet()
    {
        for ( int i = 0; i < bulletCount; i++)
        {
            GameObject go = Instantiate(bullet, transform);
            bullets.Add(go);
            bullets[i].SetActive(false);
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (bullets.Count > 0)
            {
                GameObject bul = bullets[0];

                bul.SetActive(true);

                bullets.Remove(bul);

                bul.transform.position = this.transform.position;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
}
