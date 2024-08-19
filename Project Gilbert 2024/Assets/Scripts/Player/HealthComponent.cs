using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public PlayerData playerData;

    /**
     * I think it's likely easier to just use C# built in Actions for event handling so we
     * don't have to make any adjustments in the inspector. This allows us to just invoke the
     * action during some functions and then have listeners setup checking for when the action
     * is invoked within the code. Seems easier to follow and fits what we're doing better IMO.
     **/
    public static Action OnDie;
    public static Action OnAdjustHealth;
    public static Action OnAdjustMaxHealth;

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

    public void AdjustMaxHealth (int incomingHealth)
    {
        playerData.maxHealth += incomingHealth;
        playerData.currentHealth += incomingHealth;

        OnAdjustMaxHealth?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
