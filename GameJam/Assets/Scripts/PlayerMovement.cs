using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour

{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 3f;

    bool isRunning = false;
    bool isRolling = false;
    bool isTouchingGround = false;
    Rigidbody2D myRigidBody;
    Vector2 moveInput;
    float runSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }
}
