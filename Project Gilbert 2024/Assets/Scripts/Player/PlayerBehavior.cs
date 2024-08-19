using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public PlayerData playerData;

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
    public float currentStamina;
    public float maxStamina;

    [Header("Animation")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioClip jumpSFX;
    public AudioClip climbSFX;
    public AudioClip onHitSFX;
    public AudioClip trashPickupSFX;

    public static Action OnTrashChange;

    //powell shit
    // public GameObject player; // access player
    // end of powell shit

    // Start is called before the first frame update
    void Start()
    {
        // powell shit
        // player = GameObject.Find("Player");
        // end of powell shit

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultPosition = rb2d.position;
        defaultGravityScale = rb2d.gravityScale;
        currentStamina = maxStamina;
        Reset();
    }
    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (isClimbing)
        {
            rb2d.velocity = new Vector2(horizontal * climbingMoveSpeed, vertical * climbingMoveSpeed);
            currentStamina -= Time.deltaTime;
        }
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
            currentStamina = maxStamina;
            //if(currentStamina < maxStamina)
            //{
            //    currentStamina += Time.deltaTime;
            //}
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
            }
            else if (isClimbing)
            {
                //canClimb = false;
                isClimbing = false;
                rb2d.gravityScale = defaultGravityScale;
                isClimbing = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                isJumping = true;
                animator.SetBool("isJumping", true);
                animator.SetBool("isClimbing", false);
                jumpTimeCounter = jumpTime;
                rb2d.velocity = Vector2.zero;
                rb2d.velocity = Vector2.up * playerData.jumpForce;
                numOfJumps--;
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
                animator.SetBool("isClimbing", false);
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }

            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isClimbing && canClimb && currentStamina > 0)
            {
                rb2d.gravityScale = 0;
                rb2d.velocity = Vector2.zero;
                isClimbing = true;
                isJumping = false;
                animator.SetBool("isClimbing", true);
            }
        }

        if (!isClimbing && horizontal < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        if (currentStamina < 0 && isClimbing)
        {
            currentStamina += Time.deltaTime;
            rb2d.gravityScale = defaultGravityScale;
            isClimbing = false;
            animator.SetBool("isClimbing", false);
        }
        #endregion
    }

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
                int trashValue = collision.gameObject.GetComponent<Trash>().value;
                playerData.trashAmount += trashValue;
                OnTrashChange.Invoke();
                Destroy(collision.gameObject);
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
                canClimb = false;
                isClimbing = false;
                rb2d.gravityScale = defaultGravityScale;
                isClimbing = false;
                animator.SetBool("isClimbing", false);
                Debug.Log("cannot climb");
            }
        }
    }

    public void Die()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        Debug.Log("Player dead");
    }

    // Reset all playerData to default values (maybe should be moved to different higher level class ?)
    public void ResetPlayerData()
    {
        playerData.maxHealth = 3;
        playerData.currentHealth = 3;
        playerData.isDead = false;
        playerData.items.Clear();
        playerData.jumpForce = 10;
        HealthComponent.OnAdjustHealth?.Invoke();
        playerData.trashAmount = 0;
        OnTrashChange?.Invoke();

    }

    void DeathLoop(){
        Reset();
    }

    public void Reset(){
        rb2d.position = defaultPosition;
        animator.SetBool("isDead", false);
        ResetPlayerData();
    }

    public void OnCheckpoint(){
        defaultPosition = rb2d.position;
    }

    // Remove event listener OnDisable
    public void OnDisable()
    {
        HealthComponent.OnDie -= Die;
    }

    // Add event listener OnEnable
    public void OnEnable()
    {
        HealthComponent.OnDie += Die;
    }

}
