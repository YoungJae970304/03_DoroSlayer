using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : InteractiveOb
{
    //public GameObject doro;
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
            if (Managers.Data.doros != null)
            {
                Managers.Data.doros[3].transform.position = transform.position;
                Managers.Data.doros[3].SetActive(true);
                Managers.Data.doros.RemoveAt(3);
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
