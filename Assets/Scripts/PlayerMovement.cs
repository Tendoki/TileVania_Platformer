using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private float playerDieDelay = 1f;
    [SerializeField] private AudioClip deathAudioClip;
    [SerializeField] private AudioClip killAudioClip;
    [SerializeField] private AudioClip hitShieldAudioClip;
    [SerializeField] private AudioClip fireAudioClip;

    private Vector2 moveInput;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private bool playerHasHorizontalSpeed;
    private bool playerHasVerticalSpeed;
    private CapsuleCollider2D myBodyCollider;
    private BoxCollider2D myFeetCollider;
    private float gravityScaleAtStart;
    private GameObject touchingEnemy;
    private GameObject audioHolder;
    private AudioSource audioSource;
    [SerializeField] private GameObject inRangeObject;

    private bool isMovable = true;

    void Start()
    {
        audioHolder = GameObject.Find("AudioHolder");
        audioSource = audioHolder.GetComponent<AudioSource>();

        isMovable = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isMovable)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        //Die();
    }

    public AudioClip GetFireAudioClip()
    {
        return fireAudioClip;
    }

    public AudioClip GetKillAudioClip()
    {
        return killAudioClip;
    }

    public void SetIsMovable(bool value)
    {
        isMovable = value;
        if (!isMovable)
        {
            myRigidbody.velocity = new Vector2(0, 0);
            playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isMovable)
        {
            return;
        }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if (!isMovable)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isMovable)
        {
            return;
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        myRigidbody.velocity += new Vector2(0f, jumpSpeed);
    }

    void OnInteract(InputValue value)
    {
        if (!isMovable)
        {
            return;
        }
        if (inRangeObject != null)
        {
            inRangeObject.GetComponent<Lever>().LeverSwitch();
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    private void FlipSprite()
    {
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        
        playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMovable)
        {
            if (other.tag == "Enemy" && touchingEnemy == null)
            {
                touchingEnemy = other.gameObject;
                GameSession gameSession = FindObjectOfType<GameSession>();
                if (gameSession.GetShieldLives() > 0)
                {
                    audioSource.PlayOneShot(killAudioClip, 0.7F);
                    Destroy(other.gameObject);
                    gameSession.TakeShieldLife();
                    audioSource.PlayOneShot(hitShieldAudioClip, 0.7F);
                }
                else
                {
                    StartCoroutine(DieDelay());
                }
            }
            else if (other.tag == "Hazards" && isMovable)
            {
                StartCoroutine(DieDelay());
            }
            else if (other.tag == "Lever" && isMovable)
            {
                inRangeObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Lever")
        {
            inRangeObject = null;
        }
    }

    //private void Die()
    //{
    //    if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
    //    {
    //        StartCoroutine(DieDelay());
    //    }
    //    else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
    //    {
    //        Debug.Log("fgsg");
    //        GameSession gameSession = FindObjectOfType<GameSession>();
    //        if (gameSession.GetShieldLives() > 0)
    //        {
    //            gameSession.TakeShieldLife();
    //        }
    //    }
    //}

    IEnumerator DieDelay()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        isMovable = false;
        myAnimator.SetTrigger("Dying");
        myRigidbody.velocity = deathKick;
        audioSource.PlayOneShot(deathAudioClip, 0.7F);
        yield return new WaitForSecondsRealtime(playerDieDelay);
        gameSession.ProcessPlayerDeath();
    }

    public bool GetIsMovable()
    {
        return isMovable;
    }
}
