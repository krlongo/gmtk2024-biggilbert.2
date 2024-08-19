using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // for patrolling
    [SerializeField] float moveSpeed = 5f; // adjustable enemy speed
    public float rayDist;
    private bool movingRight;
    public Transform groundDetect;

    // etc.
    Rigidbody2D rb; // initialize rigidbody
    Transform target; // set to chase the player
    Vector2 moveDirection; // logic for moving enemy to player
    public GameObject player;

    // for projectiles
    public GameObject bullet; // bullet prefab
    public float fireRate = 5000f; // fire every 5 seconds
    public float shotPower = 20f; //force of bullet

    public EnemyData enemyData;

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

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, rayDist);

        if (groundCheck.collider == false)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(BoxCollider2D) && collision.gameObject.CompareTag("Player")) // if enemy hits player
        {
            Debug.Log("Taking damage");
            player.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        }
    }
}
