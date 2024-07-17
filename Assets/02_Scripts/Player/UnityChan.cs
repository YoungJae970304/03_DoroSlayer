using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnityChan : Player
{
    public BoxCollider2D atkRange;

    public ParticleSystem atkP;

    public void EventUniAtkEnemy(int damage)
    {
        if (targets.Count > 0 && targets[0].gameObject.CompareTag("Enemy"))
        {
            Managers.Data.PlayerGage += Managers.Data.PlayerUpGauge;

            Instantiate(atkP, targets[0].transform.position, Quaternion.identity);

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);

            targets[0].GetComponent<InteractiveOb>().Hit(damage + atk);
        }
    }
}
