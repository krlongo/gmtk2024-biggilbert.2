using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public PlayerData playerData;
    public TMP_Text healthText;
    public TMP_Text trashAmountText;

    // Start is called before the first frame update
    void Start()
    {   
        HealthComponent.OnDie += PlayerDied;
        HealthComponent.OnAdjustHealth += UpdateHealth;
        PlayerBehavior.OnTrashChange += UpdateTrashAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth()
    {
        healthText.text = "Health: " + playerData.currentHealth;
    }

    private void UpdateTrashAmount()
    {
        trashAmountText.text = "Trash: " + playerData.trashAmount;
    }

    // TODO: Trigger Game Over screen
    private void PlayerDied()
    {
        Debug.Log("Game Over");
    }

}
