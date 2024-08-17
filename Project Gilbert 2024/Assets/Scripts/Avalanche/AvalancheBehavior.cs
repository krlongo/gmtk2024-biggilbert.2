using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Events;
public class AvalancheBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private float Move;
    public AvalancheData avalancheData;
    public PlayerData playerData;

    private Vector2 defaultPosition;
    

    [Header("Climbing")]
    public bool isClimbing;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultPosition = rb2d.position;
        Reset();
        HealthComponent.OnDie += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop avalanche movement if player is dead
        if(!playerData.isDead)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, avalancheData.MoveDirection * avalancheData.MoveSpeed);
        }
    }

    // Might want to move logic for collision with Player into Player class for consistency
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Colliding with player");
                playerData.isDead = true;
                HealthComponent.OnDie?.Invoke();
            }
        }
    }

    // Stop the movement of the avalanche if player is dead
    public void OnDeath()
    {
        rb2d.velocity = Vector3.zero;
        rb2d.gravityScale = 0;
        rb2d.angularDrag = 0;
        Debug.Log(rb2d.velocity);
    }


    /**This is the function you select on the GameObject with a UnityEvent. 
       This event will live on the Player OBJECT (NOT prefab), so click that & view in inspector
       Scroll down to HealthComponent & see the list of events.

       This is the reset event for the avalanch
       
       To add a new one, hit plus, select a GameObject (switch tabs to scene) NOT A PREFAB
       Then you can select the function on the script component of that GameObject.
       If it isn't showing up, try adding a pointless argument... IDK why man... IDK...
       **/
    public void Reset(){
        Debug.Log("Resetting avalanche");
        rb2d.position = defaultPosition;
        Debug.Log(defaultPosition);
    }
    
    public void OnCheckpoint(){

        Vector3 tmpPos = rb2d.position;
        tmpPos.y = tmpPos.y - 2; //how magical!
        defaultPosition = rb2d.position;
        defaultPosition = tmpPos;
    }

}
