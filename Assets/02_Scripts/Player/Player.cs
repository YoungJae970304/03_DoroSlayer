using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerClass
{
    Unitychan,
    Toko,
}

public class Player : MonoBehaviour
{
    // �̵� ���� ����
    protected float speed = 2f;     // �⺻ �̵��ӵ�
    protected float jumpForceY = 5f; // ������
    protected float jumpForceX = 0;
    protected float dashForce = 3f; // ��� �ӵ�
    protected float moveInput;      // �¿� �̵� �Է¹޴� ����

    // ���� ���� ����
    protected int atk = 1;
    protected float chargeTime = 0f;    // �����ϴ� �ð�
    protected float chargeAtk = 0.5f;   // �⺻ ���ݰ� �������� ���
    protected float fullChargeAtk = 1f; // �����ݰ� Ư�������� ���
    protected float backForce = 3f;

    // ���� ���� ����
    protected bool isGrounded = true;   // ���� ��Ҵ��� ���� ( �������� ���� )
    protected bool canDash = true;      // ����� �� �ִ��� ���� ( ��� ��Ÿ�� ���� )
    protected bool isDead = false;      // ���� ���� ����

    // ĳ���� �±� ���� ����
    List<KeyCode> numKey = new List<KeyCode>();
    public List<GameObject> player = new List<GameObject>();
    protected static Vector2 lastPos;

    // �� ���� ����
    protected List<GameObject> targets = new List<GameObject>();

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

    // ������Ʈ Ȱ��ȭ �� �ʱ�ȭ ���ִ� �۾�
    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        isGrounded = true;
        canDash = true;
        speed = 2f;
    }

    private void Update()
    {
        Debug.Log(Managers.Data.PlayerLife);
        Debug.Log(Managers.Data.PlayerGage);
        HandleInput();      // Ű �Է¿� ���� �κ��� ����ϴ� �Լ�
        ChangeInputKey();   // ĳ���� �±� �Լ�

        // ����� �ڵ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }

    protected void ChangeInputKey()
    {
        // �������� ���� �̻��϶��� ���۵ǵ��� ����
        if ( !isDead && Managers.Data.PlayerGage >= 30f )
        {
            lastPos = transform.position + new Vector3(0, 0.3f);

            for (int i = 0; i < numKey.Count; i++)
            {
                if (Input.GetKeyDown(numKey[i]))
                {
                    // �ε��� i�� ������ Ÿ������ ��ȯ���ְ� player ������ ����
                    PlayerClass playerClass = (PlayerClass)i;
                    ChangePlayer(playerClass);

                    Managers.Data.PlayerGage -= 30f;
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

    // Ű �Է� �Լ�
    protected void HandleInput()
    {
        // �¿� Ű �Է��� moveInput�� ����
        moveInput = Input.GetAxisRaw("Horizontal");

        // ĳ���Ͱ� ���� ���� ���¶�� ��, ������� ���� Ű �Է� �߻�
        if ( !isDead )
        {
            // ��Ŭ���� ������ �뽬�� �����ϸ� �뽬
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
                speed = 2f;
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

            // RŰ�� ������ ������ ȸ��
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(LifeUp());
            }

            // �ӵ�y�� ���밪�� ���� ���� ��� ��� ( 0.001 �̻��̸� ������, 0�̸� ����X )
            anim.SetFloat("doJump", Mathf.Abs(rig2d.velocity.y));

            // �ִϸ��̼� ���� ����
            // ���� �� ���(+)�� �� ��� �ִϸ��̼� ���, �ϰ�(-)�� �� �ϰ� �ִϸ��̼� ���
            anim.SetFloat("jumpVel", rig2d.velocity.y); 
        }

        if (Managers.Data.PlayerLife > 0)
        {
            anim.SetBool("doDead", false);
        }
    }

    // �̵�
    void Move()
    {
        // �¿� �Է�Ű�� ���밪�� ���� �̵��ϴ� �ִϸ��̼� ���
        anim.SetFloat("doRun", Mathf.Abs(moveInput));

        // ���� �̵��� ���õ� �ڵ�
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // �¿� �Է°��� ���� ĳ���� ��,�� ���� ����
        if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0);
        }
    }

    // ���
    void Idle()
    {
        speed = 2;
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        boxCol.enabled = true;
    }

    // ���̱�
    void Crouch()
    {
        // ���̴� �ִϸ��̼� ���
        anim.SetBool("doCrouch", true);

        // ���϶��� �ӵ��� 0���� �ؼ� �̵��Ұ�
        speed = 0f;

        // �ǰݿ� ���õ� �ݶ��̴��� ��Ȱ��ȭ
        boxCol.enabled = false;
    }

    // ����
    void Jump()
    {
        // 2�� ������ �����ϱ� ���� ���� ����
        isGrounded = false;

        // ���� ����, jumpForce��ŭ y������ ���� ����
        rig2d.AddForce(new Vector2(jumpForceX, jumpForceY), ForceMode2D.Impulse);
    }

    // �뽬
    void Dash()
    {
        // ���� �뽬�� �����ϱ� ���� ���� ����
        canDash = false;

        // �뽬 �ִϸ��̼� ���
        anim.SetTrigger("doDash");

        // �¿� ���⿡ ���� �뽬 ����
        if (moveInput >= 0)
        {
            rig2d.velocity = new Vector2(dashForce, rig2d.velocity.y);
        }
        else
        {
            rig2d.velocity = new Vector2(-dashForce, rig2d.velocity.y);
        }

        // �뽬 ��Ÿ��
        StartCoroutine(DashCoolDownCo());
    }
    IEnumerator DashCoolDownCo()
    {
        // 1�ʷ� ���� �� 1�ʰ� ������ �뽬 �����ϰ� ���ִ� ���� ����
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
    protected virtual void WeekAttack()
    {
        anim.SetTrigger("doWeekAttack");
    }

    // �ణ ������ ����
    protected virtual void CAttack()
    {
        anim.SetTrigger("doChargeAttack");
        Debug.Log("���� ����");
    }

    // �ִ� ���� ����
    protected virtual void FullCAttack()
    {
        backForce = 0;
        anim.SetTrigger("doFullCAttack");
        Debug.Log("Ǯ���� ����");
    }

    // ��� ���� �Լ�
    void Dead()
    {
        // ��� ���� ���� ����
        isDead = true;

        // ���� ������¿� ���� �ִϸ��̼� ����
        anim.SetBool("doDead", isDead);
        anim.SetTrigger("isDead");

        Debug.Log("����");
    }

    IEnumerator LifeUp()
    {
        if (Managers.Data.PlayerGage >= 70)
        {
            speed = 0;
            anim.SetTrigger("doGet");
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            Managers.Data.PlayerLife++;
            Managers.Data.PlayerGage -= 70;
        }
    }

    // �ִϸ��̼� �������� �̺�Ʈ�� �־��� �Լ���
    // �ڽ��� ���� ü�¿��� ���� ���ݷ� ��ŭ ����
    public void TakeDamage(int damage)
    {
        // �ǰ� �ִϸ��̼� ���
        anim.SetTrigger("doHit");

        // �÷��̾��� HP���� ��������ŭ ����
        Managers.Data.PlayerLife -= damage;

        // HP�� 0 ���ϸ� ���
        if (Managers.Data.PlayerLife <= 0)
        {
            Dead();
        }
    }

    // �ִϸ��̼� Ư�� �κп��� �̺�Ʈ�� ���� �Լ�
    public void EventSetMoveSpd()
    {
        speed = 2f;
        backForce = 3f;
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

    // �浹
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground �±׸� ���� ������Ʈ�� �浹�ϸ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ������ �����ϰ� �ϴ� ���� ����
            jumpForceX = 0;

            if (collision.contacts[0].normal.y >= 0.7f)
            {
                isGrounded = true;
            }

            if (collision.contacts[0].normal.x >= 0.7f)
            {
                isGrounded = true;
                jumpForceX = 3f;
            }
            else if (collision.contacts[0].normal.x <= -0.7f)
            {
                isGrounded = true;
                jumpForceX = -3f;
            }
        }

        // Item �±׸� ���� ������Ʈ�� �浹 ��
        else if (collision.gameObject.CompareTag("Item"))
        {
            // �ӵ��� 0���� �ϰ� ȹ�� �ִϸ��̼� ����
            speed = 0;
            anim.SetTrigger("doGet");
            Managers.Data._onElevator = true;
            // �浹�� ������Ʈ�� ��Ȱ��ȭ
            collision.gameObject.SetActive(false);
            Debug.Log("������ ȹ��");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Add(collision.gameObject);
        }
        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
        }
    }
}
