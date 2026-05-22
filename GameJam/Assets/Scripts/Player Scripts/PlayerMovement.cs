using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 12f;
    [SerializeField] float doubleJumpStrength = 10f;    // slightly weaker than first jump
    [SerializeField] float jumpDelay = 0.15f;
    [SerializeField] float gravityScale = 4f;
    public Transform firePoint;

    public GameObject bulletPrefab;
    public Vector3 playerPos;
    Animator myAnimator;

    bool isRunning = false;
    bool isRolling = false;
    bool isTouchingGround = false;
    bool isShooting = false;
    bool isJumping = false;
    bool canDoubleJump = false;                         // added
    public bool isKnockedBack = false;
    int groundLayer;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    Vector2 moveInput;
    float runSpeed;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        runSpeed = normalSpeed;
        groundLayer = LayerMask.GetMask("Ground","Obstacles");
        myRigidBody.gravityScale = gravityScale;
    }

    void Update()
    {
        if (!isKnockedBack) Run();
        GroundCheck();
        UpdateAnimation();
        playerPos = transform.position;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isKnockedBack) return;

        if (isTouchingGround && !isJumping)
        {
            // normal first jump
            StartCoroutine(Jump());
        }
        else if (!isTouchingGround && canDoubleJump)
        {
            // double jump in the air
            StartCoroutine(DoubleJump());
        }
    }

    void OnShield(InputValue value)
    {
        // put stuff in later
    }

    void OnAttack(InputValue value)
    {
        if (isShooting || isJumping) { return; }
        StartCoroutine(Shoot());
    }

    void Run()
    {
        myRigidBody.linearVelocityX = moveInput.x * runSpeed;
        isRunning = Mathf.Abs(myRigidBody.linearVelocityX) > Mathf.Epsilon;

        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            firePoint.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            firePoint.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void UpdateAnimation()
    {
        myAnimator.SetBool("isRunning", isRunning);
    }

    IEnumerator Shoot()
    {
        myAnimator.SetTrigger("Shooting");
        isShooting = true;

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime >= 3f / 8f && state.IsName("Player_Shoot");
        });

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpriteRenderer>().sortingOrder = 10;

        yield return new WaitUntil(() =>
        {
            return !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot");
        });

        isShooting = false;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        canDoubleJump = true;                           // enable double jump after first jump
        myRigidBody.linearVelocityY = jumpStrength;
        myAnimator.SetTrigger("Jumping");

        yield return new WaitForSeconds(jumpDelay);
        isJumping = false;
    }

    IEnumerator DoubleJump()
    {
        canDoubleJump = false;                          // use it up — no triple jump
        myRigidBody.linearVelocityY = doubleJumpStrength;
        myAnimator.SetTrigger("Jumping");               // reuses same animation

        yield return new WaitForSeconds(jumpDelay);
    }

    void GroundCheck()
    {
        bool wasInAir = !isTouchingGround;
        isTouchingGround = myBoxCollider.IsTouchingLayers(groundLayer);

        // reset double jump when landing
        if (isTouchingGround && wasInAir)
        {
            canDoubleJump = false;
        }
    }
}