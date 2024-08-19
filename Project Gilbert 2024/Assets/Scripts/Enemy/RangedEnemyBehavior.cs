using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBehavior : MonoBehaviour
{

    private float dirX;
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool facingRight = false;
    private Vector3 localeScale;

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

        localeScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        // set to move back and forth eventually
    }

    public void DecreaseHealth(int damage)
    {
        enemyData.currentHealth = enemyData.currentHealth - damage;

        if (enemyData.currentHealth <= 0)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.tag)
    }

}
