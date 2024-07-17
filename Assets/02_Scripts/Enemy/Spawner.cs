using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject doro;
    public Transform[] spawnPos;

    private void Start()
    {
        Managers.Data.doros = new List<GameObject>();

        // 생성시키고자 정한 갯수만큼 반복
        for (int i = 0; i < Managers.Data.poolSize; i++)
        {
            // 반복 수만큼 적을 생성해 enemyOb에 담고
            GameObject enemyOb = Instantiate(doro);

            // 리스트에 추가하고
            Managers.Data.doros.Add(enemyOb);

            // 위치 지정
            Managers.Data.doros[i].transform.position = spawnPos[i].position;
        }
    }
}
