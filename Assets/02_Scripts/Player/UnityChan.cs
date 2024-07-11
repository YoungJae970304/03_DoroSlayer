using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnityChan : Player
{
    public BoxCollider2D atkRange;

    public void EventUniAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            Managers.Data.PlayerGage += 10f;

            targets[0].GetComponent<Enemy>().Hit(damage + atk);

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);
        }
    }
}
