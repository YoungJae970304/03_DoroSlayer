using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    FarAttack,
    Dead
}

public class Enemy : MonoBehaviour
{
    // 몬스터 스탯 변수
    protected int atk = 1;
    protected int life = 5;
    protected int maxLife = 5;
    protected float currentTime = 1.8f;
    protected float atkCooltime = 2f;
    protected float speed = 0.75f;

    // 플레이어 정보
    protected float distance;
    protected Vector2 dir;
    protected List<GameObject> targets = new List<GameObject>();
    protected float backForce = 1f;

    // 몬스터 상태
    protected EnemyState enemyState;

    // 컴포넌트
    protected Rigidbody2D rig2d;
    protected Animator anim;
    protected SpriteRenderer sr;
    public BoxCollider2D detectRange;

    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        life = maxLife;

        enemyState = EnemyState.Idle;
    }

    protected virtual void Update()
    {
        if (targets.Count > 0)
        {
            dir = (targets[0].transform.position - transform.position).normalized;
            sr.flipX = dir.x > 0;
        }
        
        // 기본 상태
        enemyState = EnemyState.Idle;
    }

    void LateUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.FarAttack:
                FarAttack();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    void Idle()
    {
        anim.SetTrigger("doIdle");
    }

    void Move()
    {
        anim.SetTrigger("doRun");

        rig2d.velocity = new Vector2(dir.x * speed, rig2d.velocity.y);
    }

    // 데미지 적용은 공격 모션에 이벤트로 처리
    void Attack()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= atkCooltime)
        {
            anim.SetTrigger("doAttack");
            currentTime = 0;
        }
    }

    protected virtual void FarAttack()
    {
        
    }

    public void Hit(int damage)
    {
        anim.SetTrigger("doHit");
        life -= damage;

        if ( life <= 0)
        {
            enemyState = EnemyState.Dead;
        }
    }

    void Dead()
    {
        anim.SetTrigger("doDead");
        rig2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    // 애니메이션 이벤트로 넣어줄 함수들
    public void EventSetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void EventAtkPlayer()
    {
        if (targets.Count > 0)
        {
            targets[0].GetComponent<Player>().TakeDamage(atk);

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);
        }
    }
    public void EventSetDetectOn()
    {
        detectRange.enabled = true;
    }
    public void EventSetDetectOff()
    {
        detectRange.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            targets.Add( collision.gameObject );
        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targets.Remove( collision.gameObject );
        }
    }
}
