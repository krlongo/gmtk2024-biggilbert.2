using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DropBullet : MonoBehaviour
{

    // for projectile
    public GameObject bullet; // bullet prefab
    public float fireRate = 5000f; // fire every 5 seconds
    public float shotPower = 5f; //force of bullet
    private float shootingTime; // for testing fire rate (local variable)

    Transform self; // set to drop shot from current location
    Transform target; // for shooting when player is present and alive

    void Start()
    {
        target = GameObject.Find("Player").transform; // initialize target to 
        self = gameObject.GetComponentInParent<Transform>().transform; // initialize self so bird knows what to shoot
    }


    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            ShootBullet(); // set to fire continuously
        }
    }

    private void ShootBullet()
    {
        if (Time.time > shootingTime) // adjust this to activate only when target is within specific range eventually!!!
        {
            shootingTime = Time.time + fireRate / 1000; // set fire rate accordingly
            Vector2 myPos = new Vector2(self.position.x, self.position.y); //our curr position is where our muzzle points
            GameObject projectile = Instantiate(bullet, myPos, Quaternion.identity); //create our bullet where we are
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0,-1,0) * shotPower; //shoot the bullet with according power
        }
    }
}
