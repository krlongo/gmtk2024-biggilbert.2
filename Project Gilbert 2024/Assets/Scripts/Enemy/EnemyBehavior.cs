using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    public int currentHealth;
    public EnemyData enemyData; 

    // Might not be necessary, unsure if any other classes will need to listen for enemy death
    public Action OnDie;

    // for item drops
    public GameObject trash;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
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
            collision.gameObject.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
        }
    }
}
