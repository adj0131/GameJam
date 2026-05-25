using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 12f;
 //   [SerializeField] float doubleJumpStrength = 10f;    // slightly weaker than first jump
    [SerializeField] float jumpDelay = 0.15f;
    [SerializeField] float gravityScale = 4f;
    [SerializeField] float runBufferTime = 0.1f; // grace time to let player change direction before causing you to stop running animation
    [SerializeField] float parryEndLag = 0.1f; // end time where you're stuck after parrying
    [SerializeField] GameObject upgradeEffect;
    Transform enemyGroup;
    float runBufferTimer = 0f;
    public Transform firePoint;

    public GameObject bulletPrefab;
    public Vector3 playerPos;
    Animator myAnimator;
    SoundManager playerSoundManager;
    PlayerInput inputSystem;
    SceneControl sceneController;
    SpriteRenderer sr;

    public bool isRunning = false;
    bool levelEnding = false;
    bool isUpgrading = false;
    bool isParrying = false;
    public bool isTouchingGround = false;
    bool isShooting = false;
    bool isJumping = false;
  //  bool canDoubleJump = false;                         // added
    public bool isKnockedBack = false;
    bool isInvincible = false;
    int groundLayer;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    Vector2 moveInput;
    float runSpeed;


    void Start()
    {
        inputSystem = GetComponent<PlayerInput>();
        inputSystem.enabled = true;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        runSpeed = normalSpeed;
        groundLayer = LayerMask.GetMask("Ground","Obstacles");
        myRigidBody.gravityScale = gravityScale;
        playerSoundManager = GetComponent<SoundManager>();
        sceneController = FindAnyObjectByType<SceneControl>();
        sr = upgradeEffect.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        StartCoroutine(FindEnemies());
    }

    void Update()
    {
        if (!isKnockedBack) Run();
        GroundCheck();
        UpdateAnimation();
        CheckForEnemies();
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
     //   else if (!isTouchingGround && canDoubleJump)
     //   {
     //       // double jump in the air
     //       StartCoroutine(DoubleJump());
     //   }
    }

    void OnParry(InputValue value)
    {
        if (isShooting || isJumping || isUpgrading) { return; }
        StartCoroutine(Parry());
    }

    void OnAttack(InputValue value)
    {
        if (isShooting || isJumping || isUpgrading) { return; }
        StartCoroutine(Shoot());
    }

    void Run()
    {
        playerSoundManager.ManageWalkAudio();
        if(isShooting || isParrying || isUpgrading) { return; } // don't bother if you're currently shooting
        myRigidBody.linearVelocityX = moveInput.x * runSpeed;

        bool hasMovementInput = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        // checks to see if input has been zero for longer than grace period before officially stopping run
        if (hasMovementInput)
        { runBufferTimer = runBufferTime; }
        else
        { runBufferTimer -= Time.deltaTime; }
        isRunning = runBufferTimer > 0f; // Checks to see if you've run out of grace time

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
    void CheckForEnemies()
    {
        if (!levelEnding &&
            enemyGroup != null &&
            enemyGroup.childCount == 0)
        {
            levelEnding = true;
            StartCoroutine(CelebrationSequence());
        }
    }
    void UpdateAnimation()
    {
        myAnimator.SetBool("isRunning", isRunning);
    }

    IEnumerator Shoot()
    {
        playerSoundManager.playShootAudio();
        myAnimator.SetTrigger("Shooting");
        isShooting = true;
        isRunning = false;
        myRigidBody.linearVelocityX = 0f; // makes you go stationary while shooting

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
        playerSoundManager.playJumpAudio();
        isJumping = true;
     //   canDoubleJump = true;                           // enable double jump after first jump
        myRigidBody.linearVelocityY = jumpStrength;
        myAnimator.SetTrigger("Jumping");

        yield return new WaitForSeconds(jumpDelay);
        isJumping = false;
    }

  //  IEnumerator DoubleJump()
  //  {
  //      canDoubleJump = false;                          // use it up — no triple jump
  //      myRigidBody.linearVelocityY = doubleJumpStrength;
  //      myAnimator.SetTrigger("Jumping");               // reuses same animation

  //      yield return new WaitForSeconds(jumpDelay);
  //  }

    IEnumerator Parry()
    {
        isParrying = true;
        isRunning = false;
        myAnimator.SetTrigger("Parrying");
        myRigidBody.linearVelocityX = 0f;

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime >= 6f / 12f && state.IsName("Player_Parry");
        });

        isInvincible = true;

        yield return new WaitUntil(() =>
        {
            return !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Parry");
        });

        yield return new WaitForSecondsRealtime(parryEndLag); // adds a small delay so you don't immediately go back into running

        isParrying = false;
        isInvincible = false;
    }
    void GroundCheck()
    {
        bool wasInAir = !isTouchingGround;
        isTouchingGround = myBoxCollider.IsTouchingLayers(groundLayer);

        // reset double jump when landing
//        if (isTouchingGround && wasInAir)
//        {
//            canDoubleJump = false;
//        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        StopCoroutine("SlowCoroutine");
        StartCoroutine(SlowCoroutine(slowAmount, duration));
    }

    private IEnumerator SlowCoroutine(float slowAmount, float duration)
    {
        float originalSpeed = runSpeed;
        float originalJump = jumpStrength;
        float originalAnimSpeed = myAnimator.speed;

        runSpeed *= slowAmount;
        jumpStrength *= slowAmount;
        myAnimator.speed *= slowAmount;

        yield return new WaitForSeconds(duration);

        runSpeed = originalSpeed;
        jumpStrength = originalJump;
        myAnimator.speed = originalAnimSpeed;
    }

    IEnumerator CelebrationSequence()
    {
        inputSystem.enabled = false;
        myRigidBody.linearVelocityY = 0.01f; // Give just a tiny bit of right velocity to set sprite direction
        yield return null; // wait one frame
        myRigidBody.linearVelocityY = 0f; // Then set velocity to 0
        myAnimator.SetTrigger("Celebrating"); // Start the animation

        // wait for gun in the air
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime >= 24f / 35f && state.IsName("Player_Upgrade");
        });

        sr.enabled = true; // turns on the twinkle star

        // Rotate star for the rest of the animation
        while (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Upgrade"))
        {
            upgradeEffect.transform.Rotate(0f,0f,720f * Time.deltaTime);

            yield return null;
        }

        // Once all of this is done, move onto the next scene
        sceneController.LoadNextScene();
        
    }
    IEnumerator FindEnemies()
    {
        yield return null;

        GameObject enemiesObject = GameObject.Find("Enemies");

        if (enemiesObject != null)
        {
            enemyGroup = enemiesObject.transform;
        }
        else
        {
            Debug.LogError("Could not find Enemies object");
        }
    }
}