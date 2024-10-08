using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{

    public float speed;
    public float rayDist;
    private bool movingRight = true;
    public Transform groundDetect;
    public EnemyData enemyData;
    public int groundlayerIndex = 1 << 6;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(enemyData.moveSpeed * Time.deltaTime * Vector2.right);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist, groundlayerIndex);

        if (!groundCheck.collider)
        {
            TurnAround();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            TurnAround();
        }
    }

    private void TurnAround()
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
