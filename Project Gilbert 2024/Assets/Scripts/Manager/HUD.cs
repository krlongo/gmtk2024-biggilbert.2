using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerData playerData;
    public TMP_Text healthText;
    public TMP_Text trashAmountText;

    public Button resetButton;
    public GameObject deathScreen;

    public static Action OnReset;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DisableButtons()
    {
        resetButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        HealthComponent.OnDie += PlayerDied;
        HealthComponent.OnAdjustHealth += UpdateHealth;
        HealthComponent.OnAdjustMaxHealth += UpdateHealth;
        PlayerItemChange.OnTrashChange += UpdateTrashAmount;

        ResetHUD();
        resetButton.onClick.AddListener(() => ResetGame());
    }

    void OnDisable()
    {
        HealthComponent.OnDie -= PlayerDied;
        HealthComponent.OnAdjustHealth -= UpdateHealth;
        HealthComponent.OnAdjustMaxHealth -= UpdateHealth;
        PlayerItemChange.OnTrashChange -= UpdateTrashAmount;
    }

    private void UpdateHealth()
    {
        healthText.text = playerData.currentHealth.ToString();
    }

    private void UpdateTrashAmount()
    {
        trashAmountText.text = playerData.trashAmount.ToString();
    }

    // TODO: Trigger Game Over screen
    private void PlayerDied()
    {
        UpdateHealth();
        resetButton.gameObject.SetActive(true);
        deathScreen.SetActive(true);
        Debug.Log("Game Over");
    }

    private void ResetGame()
    {
        OnReset?.Invoke();
        ResetHUD();
    }

    private void ResetHUD()
    {
        DisableButtons();
        UpdateHealth();
        UpdateTrashAmount();
        deathScreen.SetActive(false);
    }

    public void Quit()
    {
       Application.Quit();
    }
}
