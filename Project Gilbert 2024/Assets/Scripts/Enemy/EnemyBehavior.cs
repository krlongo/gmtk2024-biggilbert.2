using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    public int currentHealth;
    public EnemyData enemyData;
    public Sprite deathSprite;
    public bool isDead;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;

    // for audio
    public AudioSource source;
    public AudioClip deathSound;

    // Might not be necessary, unsure if any other classes will need to listen for enemy death
    public Action OnDie;

    // for item drops
    public GameObject trash;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = deathSound;
        SetStats();
    }

    void Update()
    {
        
    }

    private void SetStats()
    {
        currentHealth = enemyData.currentHealth;
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
        source.Play();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetType() == typeof(BoxCollider2D) && collision.gameObject.CompareTag("Player")) // if enemy hits player
        {
            collision.gameObject.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        }
    }
}
