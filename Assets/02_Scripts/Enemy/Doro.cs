using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doro : Enemy
{
    private void Awake()
    {
        atkCooltime = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (targets.Count > 0)
        {
            // ���� Ȱ��ȭ �Ǿ��ִ� �÷��̾�� ������ �Ÿ�
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // ��� //
            // ���� �Ÿ� ���� �ִٸ� ���� (Move)
            if (distance > 1f)
            {
                enemyState = EnemyState.Move;
            }
            // �ٰŸ��� �ִٸ� ����
            else if (distance <= 1f)
            {
                enemyState = EnemyState.Attack;
            }
        }
    }

    protected override void Dead()
    {
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        Managers.Data.doros.Remove(this.gameObject);
        Managers.Data.doros.Add(this.gameObject);
        base.Dead();
    }
}
