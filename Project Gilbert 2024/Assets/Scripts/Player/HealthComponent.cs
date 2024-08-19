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
        if (playerData.isInvicible) return;
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
            gameObject.GetComponent<PlayerBehavior>().animator.SetBool("isDead", true);
            OnDie?.Invoke();
        } else
        {
            playerData.isInvicible = true;
            playerData.invicibleTimer = 3;
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
        playerData.isInvicible = false;
        playerData.invicibleTimer = 0;
    }

    private void Update ()
    {
        if (!playerData.isInvicible) return;
        if(playerData.invicibleTimer > 0)
        {
            playerData.invicibleTimer -= Time.deltaTime;
        } else
        {
            playerData.isInvicible = false;
        }
    }
}
