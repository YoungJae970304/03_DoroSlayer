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

        // ������Ű���� ���� ������ŭ �ݺ�
        for (int i = 0; i < Managers.Data.poolSize; i++)
        {
            // �ݺ� ����ŭ ���� ������ enemyOb�� ���
            GameObject enemyOb = Instantiate(doro);

            // ����Ʈ�� �߰��ϰ�
            Managers.Data.doros.Add(enemyOb);

            // ��ġ ����
            Managers.Data.doros[i].transform.position = spawnPos[i].position;
        }
    }
}
