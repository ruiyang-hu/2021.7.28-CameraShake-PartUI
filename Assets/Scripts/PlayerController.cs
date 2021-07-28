using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool isInvincible = false;
    public int life = 5;
    public GameObject shadowPrefab;

    [Header("左右移动")]
    private float horizontalMove;
    public float speed;
    public float accelerateTime;
    public float decelerateTime;
    private float velocityX;
    private bool canMove = true;

    [Header("触地检测")]
    public bool isOnGround;
    public LayerMask checkLayer;
    public GameObject checkPoint;
    public float checkRadius;

    [Header("跳跃")]
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool isJumping;
    public bool doubleJump = false;
    public int jumpCount;
    private bool jumpPressed;
    private bool canJump = true;

    [Header("冲刺")]
    public float dashForce;
    public float dragMaxForce;
    public float dragDuration;
    public float dashTime;// dash时长
    private float dashTimeLeft;// 冲锋剩余时间
    private float lashDashTime = -10f;// 上一次dash的时间点
    public float dashCoolDown;
    private bool isDashing;
    private Vector2 dashDirection;

    [Header("冲刺震动")]
    public int dashPause;
    public float dashStrength;

    [Header("CD的UI组件")]
    public Image cdImage;

    [Header("弹跳板")]
    public float trampolineForce;

    [Header("风扇")]
    public float fanForce;

    private bool playerIsDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        //逐帧判断是否在有跳跃次数情况下按下跳跃键
        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= lashDashTime + dashCoolDown)
            {
                //可以执行dash
                ReadyToDash();
            }
        }

        cdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;

    }

    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(checkPoint.transform.position, checkRadius, checkLayer);

        if(canMove)
            Movement();

        if(canJump)
            Jump();

        Dash();

        SwitchAnimation();
    }

    #region 角色动画切换
    void SwitchAnimation()
    {
        if (isOnGround)
        {
            animator.SetBool("falling", false);
            animator.SetBool("jumping", false);
        }
        else if(!isOnGround && rb.velocity.y > 0)
        {
            animator.SetBool("jumping", true);
        }
        else if(rb.velocity.y < 0)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("falling", true);
        }

    }
    #endregion

    #region 角色跑动
    void Movement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("running", Mathf.Abs(horizontalMove));
        if (horizontalMove > 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(horizontalMove < 0)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * Time.fixedDeltaTime * 60 * -1, ref velocityX, accelerateTime), rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref velocityX, decelerateTime), rb.velocity.y);
        }
    }
    #endregion

    #region 角色跳跃
    void Jump()
    {
        if (isOnGround)// 检测接触到地面：跳跃次数恢复；是否在跳跃为否
        {
            jumpCount = doubleJump ? 2 : 1;
            isJumping = false;
        }

        if(jumpPressed && isOnGround)// 如果在地面上且按下跳跃键：是否跳跃为是；加Y轴力；跳跃次数-1；是否按下跳跃键为否
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount --;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && !isOnGround)// 如果不在地面上且按下跳跃键且跳跃次数大于0：加Y轴力；跳跃次数-1；是否按下跳跃键为否
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

        #region 重力调整器
        if (rb.velocity.y < 0)// 当玩家下坠的时候
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;// 加速下坠
        }
        else if (rb.velocity.y > 0 && Input.GetAxis("Jump") != 1) // 当玩家上升，且没有按下跳跃键的时候
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;// 减缓上升
        }
        #endregion
    }
    #endregion

    #region 角色冲刺
    void ReadyToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;

        lashDashTime = Time.time;

        cdImage.fillAmount = 1;

        //AttackSense.Instance.HitPause(dashPause);
        AttackSense.Instance.CameraShake(dashTime, dashStrength);
        
    }
    void Dash()
    {
        Vector2 tempDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            PoolManager.Release(shadowPrefab);
            if (isDashing)
            {
                if (tempDir != Vector2.zero)
                {
                    dashDirection = tempDir;
                }
                else
                {
                    if (rb.transform.localScale.x > 0)
                    {
                        dashDirection = Vector2.right;
                    }
                    else
                    {
                        dashDirection = Vector2.left;
                    }
                }

                //将玩家当前所有的动量清零
                rb.velocity = Vector2.zero;
                //施加一个力，让玩家飞出去
                rb.AddForce(new Vector2(dashDirection.x * dashForce * 0.9f, dashDirection.y * dashForce * 0.7f), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(dashDirection.x * dashForce * 0.9f, dashDirection.y * dashForce * 0.7f);
                animator.SetBool("dashing", true);
                //StartCoroutine(DashControl());
                StartCoroutine(DashControl());
            }
            //else
            //{
            //    isDashing = false;
            //}
        }
    }

    IEnumerator DashControl()
    {
        //关闭玩家的移动和跳跃功能
        //关闭重力调整器
        isDashing = false;
        canMove = false;
        canJump = false;
        //关闭重力影响
        rb.gravityScale = -0.2f;
        // 设置无敌帧
        isInvincible = true;
        
        //施加空气阻力(Rigidbody.Drag)
        DOVirtual.Float(dragMaxForce, 0, dragDuration, RigidbodyDrag);
        //等待一段时间
        yield return new WaitForSeconds(dashTime);
        //开启所有关闭的东西
        canMove = true;
        canJump = true;
        rb.gravityScale = 1.8f;
        isInvincible = false;
    }
    public void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    // 冲刺动画控制
    public void DashingFalse()
    {
        animator.SetBool("dashing", false);
    }
    #endregion


    // 碰撞器触碰
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trampoline"))
        {
            rb.AddForce(Vector2.up * trampolineForce , ForceMode2D.Impulse);
        }
        if (other.gameObject.CompareTag("Spike"))
        {
            animator.SetTrigger("dead");
        }
    }

    // 触发器触碰
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible)
        {
            if (other.CompareTag("Spike"))
            {
                animator.SetBool("isInjured", true);
                life--;
                if (life == 0)
                {
                    PlayerDead();
                }
                isInvincible = true;
                Invoke("BackToIdle", 2f);
            }
        }

        if (other.CompareTag("Fan") && animator.GetBool("falling"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.05f);
        }
    }

    // 触发器停留
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "FanFalling")
        {
            animator.SetBool("jumping", false);
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
        }

        if (other.CompareTag("Fan"))
        {
            rb.AddForce(Vector2.up * fanForce, ForceMode2D.Impulse);
        }

    }

    // 触发器退出
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Fan")
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
    }

    // 角色死亡判断
    public void PlayerDead()
    {
        playerIsDead = true;
        GameManager.GameOver(playerIsDead);
    }

    private void BackToIdle()
    {
        isInvincible = false;
        DashingFalse();
        animator.SetBool("isInjured", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(checkPoint.transform.position, checkRadius);
    }
}
