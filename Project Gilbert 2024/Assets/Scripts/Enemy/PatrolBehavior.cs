using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{

    public float speed;
    public float rayDist;
    private bool movingRight = true;
    public Transform groundDetect;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);

        if (groundCheck.collider == false)
        {
            turnAround();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered trigger event...");
        if (collision.gameObject.CompareTag("Wall"))
        {
            turnAround();
        }
    }

    private void turnAround()
    {
        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }

}
