using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Shoot : MonoBehaviour
{

    // for projectile
    public GameObject bullet; // bullet prefab
    public float fireRate = 5000f; // fire every 5 seconds
    public float shotPower = 5f; //force of bullet
    private float shootingTime; // for testing fire rate (local variable)

    Transform self; // set to shoot from current location
    Transform target; // set to shoot x player


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform; // initialize target to player
        self = GameObject.Find("Bird").transform; // initialize self so bird knows what to shoot
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            ShootBullet(); // set to fire continuously
            // Vector3 direction = (target.position - transform.position).normalized; // direction to player
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // adjust angle of enemy
            // rb.rotation = angle; // set rotation
            // moveDirection = direction; // now we have our path
        }
    }

    private void ShootBullet()
    {
        if (Time.time > shootingTime) // adjust this to activate only when target is within specific range eventually!!!
        {
            shootingTime = Time.time + fireRate / 1000; //set the local var. to current time of shooting
            Vector2 myPos = new Vector2(self.position.x, self.position.y); //our curr position is where our muzzle points
            GameObject projectile = Instantiate(bullet, myPos, Quaternion.identity); //create our bullet
            Vector3 direction = (target.position - transform.position).normalized; //get the direction to the target
            projectile.GetComponent<Rigidbody2D>().velocity = direction * shotPower; //shoot the bullet
        }
    }
}
