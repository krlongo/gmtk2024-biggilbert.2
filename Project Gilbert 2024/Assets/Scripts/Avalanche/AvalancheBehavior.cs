using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
//using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Events;
public class AvalancheBehavior : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private float Move;
    public AvalancheData avalancheData;
    public PlayerData playerData;

    private Vector2 defaultPosition;

    public bool doNotRiseUp;

    [Header("Climbing")]
    public bool isClimbing;
    private int waveCnt = -1;
    int skip=15;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultPosition = rb2d.position;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        //For wave wiggles
        float[] waveArr = new float[] {2,4,7,9,10,8,6,3,-3,-4,-6,-7,-5,-4,-3};
        
        // Stop avalanche movement if player is dead
        if(!playerData.isDead && !doNotRiseUp)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 1 * avalancheData.moveSpeed);
        } else { 
            //wave stopped wiggle
            rb2d.gravityScale = 0;
            skip++;
            if (skip < 20){
                rb2d.velocity = new Vector2(rb2d.velocity.x,rb2d.velocity.x);
            } else {
                skip = 0;
                waveCnt++;
                float waveNum=waveArr[waveCnt%waveArr.Length];
                rb2d.velocity = new Vector2(rb2d.velocity.x,waveNum/5);
            }
        }
    }

    // Might want to move logic for collision with Player into Player class for consistency
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                playerData.isInvicible = false;
                playerData.invicibleTimer = 0;
                collision.gameObject.GetComponent<HealthComponent>().AdjustHealth(-avalancheData.damage);
            } else if(collision.gameObject.CompareTag("Checkpoint"))
            {
                doNotRiseUp = true;
            }
        }
    }

    // Stop the movement of the avalanche if player is dead
    public void OnDeath()
    {
       // rb2d.velocity = Vector3.zero;
        //rb2d.gravityScale = 0;
        doNotRiseUp=true;
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
        rb2d.position = defaultPosition;
    }

    private void OnEnable()
    {
        HealthComponent.OnDie += OnDeath;
    }

    private void OnDisable()
    {
        HealthComponent.OnDie -= OnDeath;
    }
}
