using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public List<GameObject> doros;
    public int poolSize = 4;
    public bool _onElevator = false;

    int life = 5;
    int maxLife = 5;

    public int PlayerMaxLife
    {
        get { return maxLife; }
        set
        { 
            maxLife = value; 
        }
    }

    public int PlayerLife
    {
        get { return life; }
        set
        {
            life = value;

            if (life < 0)
            {
                life = 0;
            }
            else if (life > maxLife)
            {
                life = maxLife;
            }
        }
    }

    float gage = 100;
    float maxGage = 100;

    public float PlayerMaxGauge
    {
        get { return maxGage; }
        set
        {
            maxGage = value;
        }
    }

    public float PlayerGage
    {
        get{ return gage; }
        set
        {
            gage = value;

            if (gage < 0)
            {
                gage = 0;
            }
            else if (gage > maxGage)
            {
                gage = maxGage;
            }
        }
    }

    float upGauge = 10;

    public float PlayerUpGauge
    {
        get { return upGauge; }
        set
        {
            upGauge = value;
        }
    }
}
