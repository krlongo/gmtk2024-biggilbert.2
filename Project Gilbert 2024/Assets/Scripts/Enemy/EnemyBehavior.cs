using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // for patrolling
    int maxHealth;
    int currentHealth;
    public float rayDist; // for detecting ground underneath
    private bool movingRight; // for patrolling logic
    public Transform groundDetect; // for patrolling logic

    // for movement and player interaction
    Rigidbody2D rb; // initialize rigidbody
    Transform target; // set to chase the player
    Vector2 moveDirection; // logic for moving enemy to player
    public GameObject player; // for player taking damage

    public EnemyData enemyData; // unsused

    // Might not be necessary, unsure if any other classes will need to listen for enemy death
    public Action OnDie;

    // for item drops
    public GameObject trash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // initialize rigidbody
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform; // initialize target to Player
        player = GameObject.FindGameObjectWithTag("Player"); // find player and assign to variable
        SetStats();
    }

    void Update()
    {
        
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
            Debug.Log("Taking damage");
            player.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        }
    }
}
