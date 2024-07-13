using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject upTarget, downTarget;
    bool upEleva = false;
    float speed = 5f;

    void Update()
    {
        if (Managers.Data._onElevator && upEleva)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, upTarget.transform.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, downTarget.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            upEleva = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            upEleva = false;
            collision.transform.SetParent(null);
        }
    }
}
