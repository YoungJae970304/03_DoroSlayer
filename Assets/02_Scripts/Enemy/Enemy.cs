using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    FarAttack,
    Dead
}

public class Enemy : InteractiveOb
{
    // 몬스터 스탯 변수
    protected int atk = 1;
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

    // 몰드
    public GameObject money;
    public Transform target;
    TextMeshProUGUI moneyTxt;

    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        life = maxLife;

        enemyState = EnemyState.Idle;
    }

    private void Start()
    {
        target = GameObject.Find("Canvas/Mold").transform;
        moneyTxt = GameObject.Find("Canvas/Mold/Text").GetComponent<TextMeshProUGUI>();
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

        transform.Translate(new Vector2(dir.x * speed * Time.deltaTime, 0));
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

    public override void Hit(int damage)
    {
        base.Hit(damage);
        anim.SetTrigger("doHit");
        
        if (life <= 0)
        {
            enemyState = EnemyState.Dead;
        }
    }
    protected virtual void Dead()
    {
        anim.SetTrigger("doDead");
        rig2d.velocity = Vector2.zero;
        
        int randCount = Random.Range(5, 11);
        for (int i = 0; i < randCount; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            GameObject itemFx = Instantiate(money, screenPos, Quaternion.identity);

            itemFx.transform.SetParent(GameObject.Find("Canvas").transform);
            itemFx.GetComponent<ItemFx>().Explosion(screenPos, target.position, 150f);
        }
        UIManager uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        uIManager.SetMoney(Random.Range(50, 101));
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
