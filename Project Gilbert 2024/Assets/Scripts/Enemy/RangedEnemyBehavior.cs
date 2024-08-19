using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBehavior : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private float dirX;
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool facingRight = false;
    private Vector3 localScale;

    // for base mechanics
    // Transform target; // set to chase the player
    Transform self; // set to shoot from current location
    Vector2 moveDirection; // logic for moving enemy to player
    GameObject player; // have instance of player for collision and projectile hit

    // for enemy data and death
    public EnemyData enemyData;
    public Action OnDie;

    // for enemy drops
    public GameObject trash;


    // Start is called before the first frame update
    void Start()
    {
        // target = GameObject.Find("Player").transform; // initialize target to Player (WILL BE HELPFUL FOR ATTACKING LATER)
        self = GameObject.Find("Bird").transform; // initialize self so bird knows what to shoot
        player = GameObject.FindGameObjectWithTag("Player"); // find player and assign to variable

        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        // set to move back and forth eventually
    }

    private void SetStats()
    {
        maxHealth = enemyData.maxHealth;
        currentHealth = enemyData.currentHealth;
    }

    public void DecreaseHealth(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
            Die();
        }
    }

    public void Die()
    {
        Instantiate(trash, gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(BoxCollider2D) && collision.gameObject.CompareTag("Player")) // if enemy hits player
        {
            player.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        }
    }

    // EVERYTHING UNDERNEATH HAS TO DEAL WITH PLAYER MOVEMENT BOUNCING OFF WALLS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) // if colliding with platform/wall
        {
            dirX *= -1f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * enemyData.moveSpeed, rb.velocity.y);
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    private void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

}
