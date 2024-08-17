using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class AvalancheBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private float Move;
    public float MoveSpeed;
    public int MoveDirection = 1;

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

        rb2d.velocity = new Vector2(rb2d.velocity.x, MoveDirection * MoveSpeed);
     
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
