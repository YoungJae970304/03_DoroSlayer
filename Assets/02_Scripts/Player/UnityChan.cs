using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnityChan : Player
{
    public void EventUniAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            targets[0].GetComponent<Enemy>().Hit(damage + atk);
        }
    }
}
