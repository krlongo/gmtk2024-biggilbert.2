using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class RangedEnemyBehavior : MonoBehaviour
{
    public int currentHealth;
    private float dirX;
    private float dirY;
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool facingRight = false;
    private Vector3 localScale;

    // for enemy data and death
    public EnemyData enemyData;
    public Action OnDie;

    // for enemy drops
    public GameObject trash;
    public GameObject player;

    public bool isDiving;
    public bool isReturning;
    public float divingTimer;
    public Vector3 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        dirY = -1f;
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDiving && ShouldDive())
        {
            Dive();
        }
        else if (isDiving && this.transform.position == originalPosition)
        {
            isDiving = false;
            isReturning = false;
        } 
        else if (isDiving && !isReturning && this.transform.position != originalPosition)
        {
            rb.velocity = new Vector2(0, dirY * enemyData.moveSpeed);
        } 
        else if (isDiving && isReturning && this.transform.position != originalPosition)
        {
            if (CheckDiveTimer())
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, enemyData.moveSpeed * Time.deltaTime);
            }
            else
            {
                return;
            }
        }
        else
        {
            rb.velocity = new Vector2(dirX * enemyData.moveSpeed, 0);
        }
    }

    public bool ShouldDive()
    {
        if (enemyData.stageLevel == 2 && Math.Abs(this.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < 3
            && this.transform.position.y > GameObject.FindGameObjectWithTag("Player").transform.position.y 
            && Math.Abs(this.transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y) < 3) 
        { 
            return true;
        }

        return false;
    }

    public void Dive()
    {
        isDiving = true;
        originalPosition = this.transform.position;
        rb.velocity = new Vector2(0, dirY * enemyData.moveSpeed);
        divingTimer = .5f;
    }

    // Added to make bird wait when hitting the ground before going back up
    public bool CheckDiveTimer()
    {
        if (divingTimer > 0)
        {
            divingTimer -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void SetStats()
    {
        currentHealth = enemyData.currentHealth;
        rb.velocity = new Vector2(dirX * enemyData.moveSpeed, 0);
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
            collision.gameObject.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        } else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) // if colliding with platform/wall
        {
            if (isDiving)
            {
                isReturning = true;
            }
            else
            {
                dirX *= -1f;
                rb.velocity = new Vector2(dirX * enemyData.moveSpeed, 0);
            }
        }
    }
}
