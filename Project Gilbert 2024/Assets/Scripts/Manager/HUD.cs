using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public PlayerData playerData;
    public TMP_Text healthText;

    // Start is called before the first frame update
    void Start()
    {   
        HealthComponent.OnDie += PlayerDied;
        HealthComponent.OnAdjustHealth += UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth()
    {
        healthText.text = "Health: " + playerData.currentHealth;
    }

    // TODO: Trigger Game Over screen
    private void PlayerDied()
    {
        Debug.Log("Player is dead");
    }
}
