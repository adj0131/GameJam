using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 3f;
    [SerializeField] float jumpDelay = 0.15f; // number of seconds you have to wait before trying to jump again
    public Transform firePoint;

    public GameObject bulletPrefab;
    public Vector3 playerPos;
    Animator myAnimator;

    bool isRunning = false;
    bool isRolling = false;
    bool isTouchingGround = false;
    bool isShooting = false;
    bool isJumping = false;
    int groundLayer;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    Vector2 moveInput;
    float runSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        runSpeed = normalSpeed;
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        Run();
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
        if (!isTouchingGround || isJumping)
        { return; }
        StartCoroutine(Jump());
    }
    void OnShield(InputValue value)
    {
        // put stuff in later
    }
    void OnAttack(InputValue value)
    {
        if (isShooting || isJumping)
        { return;}
        StartCoroutine(Shoot());
    }
    void Run()
    {
        myRigidBody.linearVelocityX = moveInput.x * runSpeed;
        isRunning = Mathf.Abs(myRigidBody.linearVelocityX) > Mathf.Epsilon;
        // Flip player sprite based on movement direction
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            firePoint.rotation = Quaternion.Euler(0, 0, 0);
        }
// Face left
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
            return state.normalizedTime >= 3f / 8f && state.IsName("Player_Shoot"); // allows you to move forward once you're on the shooting frame where a bullet actually appears
        });
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        yield return new WaitUntil(() =>
        {
            return !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot"); // ends once a different animation is playing
        });

        isShooting = false;
    }
    IEnumerator Jump()
    {
        isJumping = true;
        myRigidBody.linearVelocityY = jumpStrength;
        myAnimator.SetTrigger("Jumping");

        yield return new WaitForSeconds(jumpDelay);
        isJumping = false;
    }
    void GroundCheck()
    {
        isTouchingGround = myBoxCollider.IsTouchingLayers(groundLayer);
        print(isTouchingGround);
    }
}
