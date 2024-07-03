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
    // 공격도 애니메이션 블렌드로 gauge값에 따라? 그러면 Attack 하나만 있어도 됨
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
    float jumpForce = 250f;
    float lastMoveDir;

    bool isGrounded = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        playerState = PlayerState.Idle;
        rig2d.velocity = Vector2.zero;
    }

    
    void FixedUpdate()
    {
        NowStateJudge();

        switch (playerState)
        {
            case PlayerState.Idle:
                // 애니메이터에서 Run이랑 블렌드해서 따로 Idle 사용X, x속도가 0이면 자동으로 Idle
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
        if (!Input.anyKey)
        {
            playerState = PlayerState.Idle;
        }
        if (isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                playerState = PlayerState.Run;
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerState = PlayerState.Crouch;
            }
            if (Input.GetKey(KeyCode.W))
            {
                playerState = PlayerState.Jump;
            }
        }
        
    }

    void IdleState()
    {
        Debug.Log("Idle");
        boxCol.enabled = true;
    }

    void RunState()
    {
        Debug.Log("달기기");
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
        Debug.Log("숙이기");
        rig2d.velocity = Vector2.zero;
        boxCol.enabled = false;
    }

    void JumpState()
    {
        Debug.Log("점프");
        isGrounded = false;

        rig2d.AddForce(new Vector2(rig2d.velocity.x, jumpForce));
        playerState = PlayerState.Run;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
