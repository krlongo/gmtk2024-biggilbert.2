using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player; // grab player for collision effects
    public Rigidbody2D rb; // assign rb to bullet
    public float hitTimer; // to be used later for I-frames

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // find player and assign to variable
        rb = GetComponent<Rigidbody2D>(); // grant bullet a rigidbody to work with
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // if colliding with player
        {
            player.GetComponent<HealthComponent>().AdjustHealth(-1); // lower health by 1
            // hitTimer = 3f; // for I-frames later to be implemented
            Destroy(this.gameObject); // and destroy the object
        }
    }
}
