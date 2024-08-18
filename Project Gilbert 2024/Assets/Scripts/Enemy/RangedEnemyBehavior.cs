using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBehavior : MonoBehaviour
{
    // for base mechanics
    [SerializeField] float moveSpeed = 0f; // adjustable enemy speed
    Rigidbody2D rb; // initialize rigidbody
    Transform target; // set to chase the player
    Transform self; // set to shoot from current location
    Vector2 moveDirection; // logic for moving enemy to player
    GameObject player; // have instance of player for collision

    // for projectile
    public GameObject bullet; // bullet prefab
    public float fireRate = 5000f; // fire every 5 seconds
    public float shotPower = 5f; //force of bullet
    private float shootingTime; // for testing fire rate (local variable)

    // for enemy data and death
    public EnemyData enemyData;
    public Action OnDie;

    // for enemy drops
    public GameObject trash;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // initialize rigidbody
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform; // initialize target to Player
        self = GameObject.Find("RangedEnemy").transform; // initialize self to rangedEnemy shooting
        player = GameObject.FindGameObjectWithTag("Player"); // find player and assign to variable
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Shoot(); // set to fire continuously
            Vector3 direction = (target.position - transform.position).normalized; // direction to player
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // adjust angle of enemy
            // rb.rotation = angle; // set rotation
            moveDirection = direction; // now we have our path
        }
    }

    private void Shoot()
    {
        if (Time.time > shootingTime)
        {
            shootingTime = Time.time + fireRate / 1000; //set the local var. to current time of shooting
            Vector2 myPos = new Vector2(self.position.x, self.position.y); //our curr position is where our muzzle points
            GameObject projectile = Instantiate(bullet, myPos, Quaternion.identity); //create our bullet
            Vector3 direction = (target.position - transform.position).normalized; //get the direction to the target
            projectile.GetComponent<Rigidbody2D>().velocity = direction * shotPower; //shoot the bullet
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
