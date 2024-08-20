using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;
//using static Unity.VisualScripting.Member;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public PlayerData playerData;
    public Slider staminaBar;

    // TODO: Link some values below with values in playerData so it's accessible via other classes if necessary

    [Header("Movement")]
    private float horizontal;
    private float vertical;
    public float MoveSpeed;

    [Header("Jumping")]
    public int numOfJumps;
    public LayerMask groundMask;
    public bool isGrounded;
    public Transform feetPos;
    public bool isJumping;
    public float jumpTimeCounter;
    public float jumpTime;
    private float defaultGravityScale;

    private Vector3 defaultPosition;

    [Header("Climbing")]
    public float climbingMoveSpeed;
    public bool isClimbing = false;
    public bool canClimb;

    [Header("Animation")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioClip jumpSFX;
    public AudioClip climbSFX;
    public AudioClip onHitSFX;
    public AudioClip trashPickupSFX;


    // Start is called before the first frame update
    void Start()
    {

        rb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultPosition = rb2d.position;
        defaultGravityScale = rb2d.gravityScale;
        playerData.currentStamina = playerData.maxStamina;
        Reset();
    }
    private void FixedUpdate()
    {
        if (playerData.isDead) return;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (isClimbing)
        {
            rb2d.velocity = new Vector2(horizontal * climbingMoveSpeed, vertical * climbingMoveSpeed);
            playerData.currentStamina -= Time.deltaTime;

            // set audio to climbing clip loop -------------------------------------------------------------
            playerAudioSource.clip = climbSFX; //  set to play climbing audio in audioSource
            if(!playerAudioSource.isPlaying)
            {
                playerAudioSource.Play();
            }
        } // --------------------------------------------------------------------------------------------------
        else
        {
            rb2d.velocity = new Vector2(horizontal * MoveSpeed, rb2d.velocity.y);
        }

        if(!Mathf.Approximately(horizontal, 0.0f) && isGrounded)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if(isGrounded)
        {
            playerData.currentStamina = playerData.maxStamina;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Don't allow movement if player is dead (likely better way to do this to stop update call)
        if (playerData.isDead) return;

        #region Jumping/Falling
        isGrounded = Physics2D.OverlapCircle(feetPos.position, .3f, groundMask);

        if (rb2d.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if(isGrounded)
            {
                isJumping = true;
                animator.SetBool("isJumping", true);
                jumpTimeCounter = jumpTime;
                rb2d.velocity = Vector2.zero;
                rb2d.velocity = Vector2.up * playerData.jumpForce;
                numOfJumps--;
                playerAudioSource.clip = jumpSFX; // set audioSource to jump
                playerAudioSource.Play();

            }
            else if (isClimbing)
            {
                //canClimb = false;
                isClimbing = false;
                rb2d.gravityScale = defaultGravityScale;
                playerAudioSource.Stop();
                isClimbing = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                isJumping = true;
                animator.SetBool("isJumping", true);
                animator.SetBool("isClimbing", false);
                jumpTimeCounter = jumpTime;
                rb2d.velocity = Vector2.zero;
                rb2d.velocity = Vector2.up * playerData.jumpForce;
                numOfJumps--;
                playerAudioSource.clip = jumpSFX; // set audioSource to jump
                playerAudioSource.Play();
            }
        }

        if(Input.GetKey(KeyCode.Space) && isJumping)
        {
            if(jumpTimeCounter > 0)
            {
                rb2d.velocity = Vector2.up * playerData.jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                animator.SetBool("isJumping", false);

            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
        #endregion

        #region Climbing
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if(isClimbing)
            {
                rb2d.gravityScale = defaultGravityScale;
                isClimbing = false;
                playerAudioSource.Stop();
                animator.SetBool("isClimbing", false);
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                staminaBar.gameObject.SetActive(false);
            }

            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isClimbing && canClimb && playerData.currentStamina > 0)
            {
                rb2d.gravityScale = 0;
                rb2d.velocity = Vector2.zero;
                isClimbing = true;
                isJumping = false;
                staminaBar.gameObject.SetActive(true);
                animator.SetBool("isClimbing", true);
            }
        }

        if (!isClimbing && horizontal < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        if ((playerData.currentStamina < 0 && isClimbing) || !insideClimbingArea)
        {
            playerData.currentStamina += Time.deltaTime;
            rb2d.gravityScale = defaultGravityScale;
            isClimbing = false;
            staminaBar.gameObject.SetActive(false);

            animator.SetBool("isClimbing", false);
        }

        staminaBar.value = playerData.currentStamina / 100;
        staminaBar.maxValue = playerData.maxStamina / 100;
        #endregion
    }

    public bool insideClimbingArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Climbable"))
            {
                canClimb = true;
                Debug.Log("can climb");
            }
            else if (collision.gameObject.CompareTag("Trash"))
            {
                if (collision.gameObject.GetComponent<Trash>().isGrabbable)
                {
                    int trashValue = collision.gameObject.GetComponent<Trash>().value;
                    playerData.trashAmount += trashValue;
                    PlayerItemChange.OnTrashChange?.Invoke();
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Climbable"))
            {
                canClimb = true;
                insideClimbingArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Climbable"))
            {
                if(isClimbing)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                }
                else
                {
                    if(insideClimbingArea)
                    {
                        canClimb = false;
                    }
                }
                if(!insideClimbingArea)
                {
                    canClimb = false;
                    isClimbing = false;
                    playerAudioSource.Stop();
                    staminaBar.gameObject.SetActive(false);
                    rb2d.gravityScale = defaultGravityScale;
                    isClimbing = false;
                    animator.SetBool("isClimbing", false);
                }
                else
                {
                    insideClimbingArea = false;
                }

            }
        }
    }

    public void Die()
    {
        playerData.currentLevel = 0;
        rb2d.velocity = Vector2.zero;
    }

    // Reset all playerData to default values (maybe should be moved to different higher level class ?)
    public void ResetPlayerData()
    {
        playerData.maxHealth = 3;
        playerData.currentHealth = playerData.maxHealth;
        HealthComponent.OnAdjustHealth?.Invoke();
        playerData.isDead = false;
        playerData.items.Clear();
        playerData.jumpForce = 10;
        playerData.trashAmount = 0;
        playerData.maxStamina = 3;
        PlayerItemChange.OnTrashChange?.Invoke();
        playerData.currentLevel = 1;
    }

    void DeathLoop(){
        Reset();
    }

    public void Reset(){
        rb2d.position = defaultPosition;
        animator.SetBool("isDead", false);
    }

    public void OnCheckpoint(){
        defaultPosition = rb2d.position;
    }

    public void StopPlayer()
    {
        rb2d.velocity = Vector2.zero;
    }

    // Remove event listener OnDisable
    public void OnDisable()
    {
        HealthComponent.OnDie -= Die;
        BonfireBehavior.OnRest -= StopPlayer;
        HUD.OnReset -= ResetPlayerData;
    }

    // Add event listener OnEnable
    public void OnEnable()
    {
        PlayerItemChange.OnTrashChange?.Invoke();
        HealthComponent.OnDie += Die;
        BonfireBehavior.OnRest += StopPlayer;
        HUD.OnReset += ResetPlayerData;
    }
}
