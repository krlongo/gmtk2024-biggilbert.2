using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D playerRb;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // for stomping mechanic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") & collision.GetType() == typeof(CapsuleCollider2D))
        {
            if (collision.gameObject.GetComponent<EnemyBehavior>())
            {
                if(!collision.gameObject.GetComponent<EnemyBehavior>().isDead)
                {
                    collision.gameObject.GetComponent<EnemyBehavior>().DecreaseHealth(1);
                }
            }
            else
            {
                if(collision.gameObject.GetComponent<RangedEnemyBehavior>())
                {
                    if (!collision.gameObject.GetComponent<RangedEnemyBehavior>().isDead)
                    {
                        collision.gameObject.GetComponent<RangedEnemyBehavior>().DecreaseHealth(1);
                    }
                }
            }
            playerData.currentStamina = playerData.maxStamina;
            playerRb.velocity = new Vector2(playerRb.velocity.x, bounce);
        }
    }
}
