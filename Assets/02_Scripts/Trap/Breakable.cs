using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject doro;
    float randVal;

    private void Start()
    {
        randVal = Random.value;
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
}
