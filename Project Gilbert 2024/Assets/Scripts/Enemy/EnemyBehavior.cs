using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // adjustable enemy speed
    Rigidbody2D rb; // initialize rigidbody
    Transform target; // set to chase the player
    Vector2 moveDirection; // logic for moving enemy to player

    public GameObject bullet; // bullet prefab
    public float fireRate = 5000f; // fire every 5 seconds
    public float shotPower = 20f; //force of bullet

    public EnemyData enemyData;

    // Might not be necessary, unsure if any other classes will need to listen for enemy death
    public Action OnDie;

    public GameObject trash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // initialize rigidbody
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform; // initialize target to Player


    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized; // direction to player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // adjust angle of enemy
            rb.rotation = angle; // set rotation
            moveDirection = direction; // now we have our path
        }
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

    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed; // set to chase player
        }
    }

}
