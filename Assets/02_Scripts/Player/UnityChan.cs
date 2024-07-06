using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnityChan : Player
{
    private void Start()
    {
        CurrentHP = maxHP;
        StartCoroutine(Hpup());
    }

    IEnumerator Hpup()
    {
        while ( true )
        {
            CurrentHP += 10;
            yield return new WaitForSeconds(5f);
        }
    }
}
