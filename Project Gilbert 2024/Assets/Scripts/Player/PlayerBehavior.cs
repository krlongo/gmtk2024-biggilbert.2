using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

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
    public float jumpForce;
    public int numOfJumps;

    private Vector3 defaultPosition;

    [Header("Climbing")]
    public bool isClimbing = false;
    public bool canClimb;

    public static Action OnTrashChange;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultPosition = rb2d.position;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't allow movement if player is dead (likely better way to do this to stop update call)
        if (playerData.isDead) return;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(isClimbing)
        {
            rb2d.velocity = new Vector2(horizontal * MoveSpeed, vertical * MoveSpeed);
        }
        else
        {
            rb2d.velocity = new Vector2(horizontal * MoveSpeed, rb2d.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isClimbing)
            {
                //canClimb = false;
                isClimbing = false;
                rb2d.gravityScale = 1;
                isClimbing = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                isJumping = true;
                animator.SetBool("isJumping", true);
                animator.SetBool("isClimbing", false);
                jumpTimeCounter = jumpTime;
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpForce));
                numOfJumps--;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(!isClimbing && canClimb)
            {
                rb2d.gravityScale = 0;
                rb2d.velocity = Vector2.zero;
                isClimbing = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            if(isClimbing)
            {
                rb2d.gravityScale = 1;
                isClimbing = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);

            }
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up * 2);

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.gameObject.CompareTag("Ground"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.CompareTag("Climbable"))
            {
                canClimb = true;
                Debug.Log("can climb");
            }
            else if (collision.gameObject.CompareTag("Trash"))
            {
                int trashValue = collision.gameObject.GetComponent<Trash>().value;
                playerData.trashAmount += trashValue;
                OnTrashChange.Invoke();
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
                rb2d.gravityScale = 1;
                isClimbing = false;
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
    }

    void DeathLoop(){
        Reset();
    }

    public void Reset(){
        rb2d.position = defaultPosition;
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
