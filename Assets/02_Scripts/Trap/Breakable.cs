using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : InteractiveOb
{
    public GameObject doro;
    float randVal;
    

    private void Start()
    {
        randVal = Random.value;
        life = 2;
    }

    private void OnDisable()
    {
        if (randVal < 0.5f)
        {
            if (doro != null)
            {
                doro.transform.position = transform.position;
                doro.SetActive(true);
            }
        }
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);

        if (life <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
