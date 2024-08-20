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
    public bool isDead;
    private float dirX;
    private float dirY;
    private Rigidbody2D rb;
    public float moveSpeed;
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

    public Sprite deathSprite;

    public BoxCollider2D boxCollider;

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
        if (!isDiving && ShouldDive() && !isDead)
        {
            Dive();
        }
        else if (isDiving && this.transform.position == originalPosition)
        {
            isDiving = false;
            isReturning = false;
            CheckDirection();
        } 
        else if (isDiving && !isReturning && this.transform.position != originalPosition)
        {
            rb.velocity = new Vector2(0, dirY * enemyData.moveSpeed);
        } 
        else if (isDiving && isReturning && this.transform.position != originalPosition)
        {
            if (CheckDiveTimer())
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, enemyData.moveSpeed * Time.deltaTime);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (!isDead)
                rb.velocity = new Vector2(dirX * enemyData.moveSpeed, 0);
        }
    }

    public bool ShouldDive()
    {
        if (enemyData.stageLevel == 2 && Math.Abs(this.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < 2
            && this.transform.position.y > GameObject.FindGameObjectWithTag("Player").transform.position.y 
            && Math.Abs(this.transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y) < 4) 
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
        transform.eulerAngles = new Vector3(0, 0, -90);
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
        isDead = false;
        CheckDirection();
    }

    public void DecreaseHealth(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
            Die();
        }
    }

    public void Die()
    {
        Instantiate(trash, gameObject.transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().sprite = deathSprite;
        GetComponent<Animator>().enabled = false;
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 10;
        rb.mass = 1;
        boxCollider.isTrigger = true;
        StartCoroutine(DestroySelf(3f));
    }

    public IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void CheckDirection()
    {
        if (dirX > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(BoxCollider2D) && collision.gameObject.CompareTag("Player")) // if enemy hits player
        {
            collision.gameObject.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        } else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy")) // if colliding with platform/wall
        {
            if (isDiving)
            {
                isReturning = true;
            }
            else
            {
                dirX *= -1f;
                CheckDirection();
                rb.velocity = new Vector2(dirX * enemyData.moveSpeed, 0);
            }
        }
    }
}
