using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public PlayerData playerData;

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

    /**
     * I think it's likely easier to just use C# built in Actions for event handling so we
     * don't have to make any adjustments in the inspector. This allows us to just invoke the
     * action during some functions and then have listeners setup checking for when the action
     * is invoked within the code. Seems easier to follow and fits what we're doing better IMO.
     **/
    public static Action OnDie;
    public static Action OnAdjustHealth;

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

    // Adjust current Health based on incoming healing (positive incomingHealth) or damage (negative incomingHealth)
    public void AdjustHealth (int incomingHealth)
    {
        playerData.currentHealth += incomingHealth;
        Debug.Log(playerData.currentHealth); // for testing health being lowered
        if (playerData.currentHealth > playerData.maxHealth)
        {
            playerData.currentHealth = playerData.maxHealth;
        }
        OnAdjustHealth?.Invoke();
        if (playerData.currentHealth <= 0)
        {
            playerData.isDead = true;
            OnDie?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerData.isDead){
            onPlayerDeath.Invoke();
        }
    }
}
