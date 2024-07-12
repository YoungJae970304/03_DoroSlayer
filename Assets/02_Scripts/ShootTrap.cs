using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : MonoBehaviour
{
    public GameObject bullet;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 1f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        GameObject go = Instantiate(bullet);
        go.transform.position = target.position;
    }
}
