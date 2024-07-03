using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Crouch,
    Jump,
    // ���ݵ� �ִϸ��̼� ����� gauge���� ����? �׷��� Attack �ϳ��� �־ ��
    WeekAttack,
    MiddleAttack,
    StrongAttack,
    Hitted,
    Dead,
    Positive
}

public class PlayerController : MonoBehaviour
{
    PlayerState playerState;

    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rig2d;
    BoxCollider2D boxCol;

    float speed = 1f;
    float jumpForce = 300f;
    float lastMoveDir;

    bool isGrounded = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        playerState = PlayerState.Idle;
    }

    
    void Update()
    {
        NowStateJudge();

        switch (playerState)
        {
            case PlayerState.Idle:
                // �ִϸ����Ϳ��� Run�̶� �����ؼ� ���� Idle ���X, x�ӵ��� 0�̸� �ڵ����� Idle
                IdleState();
                break;
            case PlayerState.Run:
                RunState();
                break;
            case PlayerState.Crouch:
                CrouchState();
                break;
            case PlayerState.Jump:
                JumpState();
                break;
            case PlayerState.WeekAttack:
                
                break;
            case PlayerState.MiddleAttack:
                
                break;
            case PlayerState.StrongAttack:
                
                break;
            case PlayerState.Hitted:
                
                break;
            case PlayerState.Dead:
                
                break;
            case PlayerState.Positive:
                
                break;
        }
    }

    void NowStateJudge()
    {
        if (Input.GetAxis("Horizontal") != 0 && isGrounded)
        {
            playerState = PlayerState.Run;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerState = PlayerState.Crouch;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerState = PlayerState.Jump;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
    }

    void IdleState()
    {
        rig2d.velocity = Vector2.zero;
    }

    void RunState()
    {
        // moveInput ���� ���� Idle���۰� Run������ ����
        float moveInput = Input.GetAxisRaw("Horizontal");

        rig2d.velocity = new Vector2(moveInput * speed, rig2d.velocity.y);

        if (moveInput != 0)
        {
            lastMoveDir = moveInput;
        }
        sr.flipX = lastMoveDir < 0;
    }

    void CrouchState()
    {
        rig2d.velocity = Vector2.zero;
        boxCol.enabled = false;
    }

    void JumpState()
    {
        isGrounded = false;

        rig2d.AddForce(new Vector2(rig2d.velocity.x, jumpForce));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
