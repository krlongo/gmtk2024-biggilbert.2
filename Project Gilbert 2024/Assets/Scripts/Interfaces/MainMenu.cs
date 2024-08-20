using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject controlsMenu;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        ResetPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerData()
    {
        playerData.maxHealth = 3;
        playerData.currentHealth = playerData.maxHealth;
        playerData.isDead = false;
        playerData.items.Clear();
        playerData.jumpForce = 10;
        playerData.trashAmount = 0;
        playerData.maxStamina = 3;
        playerData.currentLevel = 0;
    }

    public void ShowControls()
    {
        controlsMenu.SetActive(!controlsMenu.activeSelf);
    }
}
