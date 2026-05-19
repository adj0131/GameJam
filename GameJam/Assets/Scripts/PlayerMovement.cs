using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour

{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 3f;
    [SerializeField] Transform firePoint;

    bool isRunning = false;
    bool isRolling = false;
    bool isTouchingGround = false;
    Rigidbody2D myRigidBody;
    Vector2 moveInput;
    float runSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        runSpeed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isTouchingGround)
        { return; }
        myRigidBody.linearVelocityY = jumpStrength;
    }
    void OnShield(InputValue value)
    {
        // put stuff in later
    }
    void OnAttack(InputValue value)
    {
        // put stuff in later
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
}
