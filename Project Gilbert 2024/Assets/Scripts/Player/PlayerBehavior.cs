using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private float Move;
    public float MoveSpeed;

    public float jumpForce;
    public int numOfJumps;

    [Header("Climbing")]
    public bool isClimbing;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        rb2d.velocity = new Vector2(Move * MoveSpeed, rb2d.velocity.y);
        

        if(Input.GetKeyDown(KeyCode.Space)) 
        { 
            //if(numOfJumps > 0)
            {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpForce));
                numOfJumps--;
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {

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
}
