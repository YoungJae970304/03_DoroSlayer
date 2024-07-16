using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveOb : MonoBehaviour
{
    protected int life = 5;
    protected int maxLife = 5;

    public virtual void Hit(int damage)
    {
        life -= damage;
    }
}
