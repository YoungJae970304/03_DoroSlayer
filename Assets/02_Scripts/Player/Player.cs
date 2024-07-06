using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Unitychan,
    Toko,
}

public class Player : MonoBehaviour
{
    // �̵� ���� ����
    protected float speed = 1f;     // �⺻ �̵��ӵ�
    protected float jumpForce = 4f; // ������
    protected float dashForce = 2f; // ��� �ӵ�
    protected float moveInput;      // �¿� �̵� �Է¹޴� ����

    // ���� ���� ����
    protected float chargeTime = 0f;    // �����ϴ� �ð�
    protected float chargeAtk = 0.5f;   // �⺻ ���ݰ� �������� ���
    protected float fullChargeAtk = 1f; // �����ݰ� Ư�������� ���

    // ���� ���� ����
    protected bool isGrounded = true;   // ���� ��Ҵ��� ���� ( �������� ���� )
    protected bool canDash = true;      // ����� �� �ִ��� ���� ( ��� ��Ÿ�� ���� )
    protected bool isDead = false;

    // ĳ���� �±� ���� ����
    List<KeyCode> numKey = new List<KeyCode>();
    public List<GameObject> player = new List<GameObject>();
    protected static Vector2 lastPos;

    // ������Ʈ
    protected Rigidbody2D rig2d;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected BoxCollider2D boxCol;

    protected void Start()
    {
        numKey.Add(KeyCode.Alpha1);
        numKey.Add(KeyCode.Alpha2);
    }

    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        isGrounded = true;
        canDash = true;
        speed = 1f;
    }

    private void Update()
    {
        Debug.Log(Managers.Data.playerLife);
        HandleInput();
        ChangeInputKey();
    }

    protected void ChangeInputKey()
    {
        // ���� ���ǹ����� ������ �߰��ؼ� �������� ���� �̻��϶��� ���۵ǵ��� ����
        if ( !isDead )
        {
            lastPos = transform.position + new Vector3(0, 0.3f);

            for (int i = 0; i < numKey.Count; i++)
            {
                if (Input.GetKeyDown(numKey[i]))
                {
                    // �ε��� i�� ������ Ÿ������ ��ȯ���ְ� player ������ ����
                    PlayerClass playerClass = (PlayerClass)i;
                    ChangePlayer(playerClass);
                }
            }
        }
    }

    void ChangePlayer(PlayerClass selectedPlayer)
    {
        // ��� ĳ���͸� ��Ȱ��ȭ�ϰ� ���õ� ĳ���͸� Ȱ��ȭ
        for(int i = 0;i < player.Count; i++)
        {
            // ���õ� ĳ���ʹ� ������ ��ġ�� �̵�
            if (i == (int)selectedPlayer)
            {
                player[i].gameObject.SetActive(true);
                player[i].transform.position = lastPos;
            }
            else
            {
                player[i].gameObject.SetActive(false);
            }
        }
    }

    protected void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if ( !isDead )
        {
            // ��Ŭ���� ������ ��ð� �����ϸ� ���
            if (Input.GetMouseButtonDown(1) && canDash)
            {
                Dash();
            }

            // �¿� �̵� �Է��� �ִٸ� �̵� / ���ٸ� ���
            if (moveInput != 0)
            {
                Move();
            }
            else
            {
                Idle();
            }

            // ���� �ְ� SŰ�� ������ ���̱�
            if (Input.GetKey(KeyCode.S) && isGrounded)
            {
                Crouch();
            }
            // S�� ���� ���� ���·�
            else if (Input.GetKeyUp(KeyCode.S) && isGrounded)
            {
                anim.SetBool("doCrouch", false);
                speed = 1f;
            }

            // �����ְ� Space ������ ����
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            // ���콺 ������ ������ ������ ���� ����
            if (Input.GetMouseButton(0))
            {
                StartCharge();
            }
            // ���콺 ������ ���� ������ ���� ����
            if (Input.GetMouseButtonUp(0))
            {
                Attack();
            }

            anim.SetFloat("jumpVel", rig2d.velocity.y);
            anim.SetFloat("doJump", Mathf.Abs(rig2d.velocity.y));
        }

        if (Managers.Data.playerLife > 0)
        {
            anim.SetBool("doDead", false);
        }
    }

    // �̵�
    void Move()
    {
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        transform.Translate(Vector2.right * moveInput * speed * Time.deltaTime);
        sr.flipX = moveInput < 0;
    }

    // ���
    void Idle()
    {
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        Debug.Log("���");
        boxCol.enabled = true;
    }

    // ���̱�
    void Crouch()
    {
        anim.SetBool("doCrouch", true);
        speed = 0f;
        Debug.Log("�ɱ�");
        boxCol.enabled = false;
    }

    // ����
    void Jump()
    {
        isGrounded = false;
        rig2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    // ���
    void Dash()
    {
        canDash = false;

        anim.SetTrigger("doDash");

        if (!sr.flipX)
        {
            //rig2d.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
            rig2d.velocity = new Vector2(dashForce, rig2d.velocity.y);
        }
        else
        {
            //rig2d.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
            rig2d.velocity = new Vector2(-dashForce, rig2d.velocity.y);
        }
        StartCoroutine(DashCoolDownCo());
    }
    IEnumerator DashCoolDownCo()
    {
        yield return new WaitForSeconds(1f);
        canDash = true;
    }

    // ���� ����, ���� �ð� ����
    void StartCharge()
    {
        chargeTime += Time.deltaTime;
    }

    // ���� �ð��� ���� ����
    protected virtual void Attack()
    {
        speed = 0;
        if (chargeTime >= fullChargeAtk)
        {
            FullCAttack();
        }
        else if (chargeTime >= chargeAtk)
        {
            CAttack();
        }
        else
        {
            WeekAttack();
        }

        // ���� �� ���� �ð� �ʱ�ȭ
        chargeTime = 0f;
    }

    // ���� ���� �⺻ ����
    protected void WeekAttack()
    {
        anim.SetTrigger("doWeekAttack");
        Debug.Log("�⺻ ����");
    }

    // �ణ ������ ����
    protected void CAttack()
    {
        anim.SetTrigger("doChargeAttack");
        Debug.Log("���� ����");
    }

    // �ִ� ���� ����
    protected void FullCAttack()
    {
        anim.SetTrigger("doFullCAttack");
        Debug.Log("Ǯ���� ����");
    }

    // �ڽ��� ���� ü�¿��� ���� ���ݷ� ��ŭ ����
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("doHit");

        Managers.Data.playerLife -= damage;

        if (Managers.Data.playerLife <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        isDead = true;

        anim.SetBool("doDead", isDead);
        anim.SetTrigger("isDead");

        Debug.Log("����");
    }

    // ���� �ִϸ��̼� �������� �̺�Ʈ�� �־��� �Լ� ( �ӵ��� 1�� ����� �ٽ� �����̰� )
    public void EventSetMoveSpd()
    {
        speed = 1f;
    }
    public void EventSetAlive()
    {
        isDead = false;
    }
    public void EventSetDeadTimeScale()
    {
        Time.timeScale = 0;
    }
    public void EventSetAliveTimeScale()
    {
        Time.timeScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Item"))
        {
            speed = 0;
            anim.SetTrigger("doGet");
            collision.gameObject.SetActive(false);
            Debug.Log("������ ȹ��");
        }
    }
}
