using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public bool isDeadTemp;

    /**The Unity Event... gosh what a doozy for something that should've been so simple.
    But alas, here we are.
    This event will live on the Player OBJECT (NOT prefab), so click that & view in inspector
    Scroll down to HealthComponent & see the list of events.
    To add a new one, hit plus, select a GameObject (switch tabs to scene) NOT A PREFAB
    Then you can select the function on the script component of that GameObject.
    If it isn't showing up, try adding a pointless argument... IDK why man... IDK...
    **/
    [Header("Here is death event")]
    public UnityEvent onPlayerDeath;

    [Header("Here is checkpoint event")]
    public UnityEvent onPlayerCheckpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.CompareTag("Checkpoint"))
            {
                Debug.Log("Checking Point");
                onPlayerCheckpoint.Invoke();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeadTemp){
            onPlayerDeath.Invoke();
            isDeadTemp = false;
        }
    }
}
