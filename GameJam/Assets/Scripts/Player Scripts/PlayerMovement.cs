using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpStrength = 12f;
    [SerializeField] float jumpDelay = 0.15f;
    [SerializeField] float gravityScale = 4f;
    [SerializeField] float runBufferTime = 0.1f;
    [SerializeField] float parryEndLag = 0.1f;
    [SerializeField] GameObject upgradeEffect;
    float runBufferTimer = 0f;
    public Transform firePoint;

    public GameObject bulletPrefab;
    public Vector3 playerPos;
    Animator myAnimator;
    SoundManager playerSoundManager;
    PlayerInput inputSystem;
    SceneControl sceneController;
    PlayerGunUpgrades gunUpgrades; // <-- NEW

    public bool isRunning = false;
    bool isUpgrading = false;
    bool isParrying = false;
    public bool isTouchingGround = false;
    bool isShooting = false;
    bool isJumping = false;
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
        gunUpgrades = GetComponent<PlayerGunUpgrades>(); // <-- NEW
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
            StartCoroutine(Jump());
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
        if(isShooting || isParrying || isUpgrading) { return; }
        myRigidBody.linearVelocityX = moveInput.x * runSpeed;

        bool hasMovementInput = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        if (hasMovementInput)
        { runBufferTimer = runBufferTime; }
        else
        { runBufferTimer -= Time.deltaTime; }
        isRunning = runBufferTimer > 0f;

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

    public void Celebration()
    {
        StartCoroutine(CelebrationSequence());
    }

    // ─── SHOOT ────────────────────────────────────────────────────────────────

    IEnumerator Shoot()
    {
        playerSoundManager.playShootAudio();
        myAnimator.SetTrigger("Shooting");
        isShooting = true;
        isRunning = false;
        myRigidBody.linearVelocityX = 0f;

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime >= 3f / 8f && state.IsName("Player_Shoot");
        });

        // Triple shot if unlocked, otherwise single
        if (gunUpgrades != null && gunUpgrades.hasTripleShot)
            SpawnTripleShot();
        else
            SpawnSingleBullet(firePoint.rotation);

        yield return new WaitUntil(() =>
        {
            return !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot");
        });

        isShooting = false;
    }

    void SpawnSingleBullet(Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
        bullet.GetComponent<SpriteRenderer>().sortingOrder = 10;
        SetupBullet(bullet);
    }

    void SpawnTripleShot()
    {
        float spread = gunUpgrades.spreadAngle;
        float[] angles = { 0f, spread, -spread };

        foreach (float angle in angles)
        {
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0f, 0f, angle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<SpriteRenderer>().sortingOrder = 10;
            SetupBullet(bullet);
        }
    }

    // Applies the ice flag to the bullet if the upgrade is unlocked
    void SetupBullet(GameObject bullet)
    {
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null && gunUpgrades != null)
            b.isIce = gunUpgrades.hasIceShot;
    }

    // ──────────────────────────────────────────────────────────────────────────

    IEnumerator Jump()
    {
        playerSoundManager.playJumpAudio();
        isJumping = true;
        myRigidBody.linearVelocityY = jumpStrength;
        myAnimator.SetTrigger("Jumping");

        yield return new WaitForSeconds(jumpDelay);
        isJumping = false;
    }

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

        yield return new WaitForSecondsRealtime(parryEndLag);

        isParrying = false;
        isInvincible = false;
    }

    void GroundCheck()
    {
        isTouchingGround = myBoxCollider.IsTouchingLayers(groundLayer);
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
        myRigidBody.linearVelocityY = 0.01f;
        yield return null;
        myRigidBody.linearVelocityY = 0f;
        myAnimator.SetTrigger("Celebrating");

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime >= 24f / 35f && state.IsName("Player_Upgrade");
        });

        SpriteRenderer sr = upgradeEffect.GetComponent<SpriteRenderer>();
        sr.enabled = true;

        while (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Upgrade"))
        {
            upgradeEffect.transform.Rotate(0f, 0f, 720f * Time.deltaTime);
            yield return null;
        }

        sceneController.LoadNextScene();
    }
}