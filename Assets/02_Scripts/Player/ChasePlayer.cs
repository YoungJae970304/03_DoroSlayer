using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform child;

    void Update()
    {
        child = GameObject.FindWithTag("Player").transform;
        gameObject.transform.position = child.position;
    }
}
