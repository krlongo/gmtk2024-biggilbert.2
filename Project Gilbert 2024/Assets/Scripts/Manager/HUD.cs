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

    public Button healthIncreaseButton;
    public Button jumpHeightIncreaseButton;
    public Button resetButton;

    public static Action OnReset;
    public static Action OnContinue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddHealthItem()
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        ItemModifier modifier = ScriptableObject.CreateInstance<ItemModifier>();
        modifier.stat = Stats.Health;
        modifier.modifierValue = 1;

        item.itemName = "Tin Can";
        item.description = "Health Up";
        item.modifier = modifier;
        item.cost = 10;

        if (playerData.trashAmount < item.cost)
        {
            Debug.Log("Cannot buy");
        }
        else
        {
            playerData.items.Add(item);
            BonfireBehavior.OnAddItem?.Invoke(item);
            DisableButtons();
            OnContinue?.Invoke();
        }
    }

    public void AddJumpHeightItem()
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        ItemModifier modifier = ScriptableObject.CreateInstance<ItemModifier>();
        modifier.stat = Stats.JumpHeight;
        modifier.modifierValue = .5f;

        item.itemName = "Spring Boots";
        item.description = "Jump Height Up";
        item.modifier = modifier;
        item.cost = 20;

        if (playerData.trashAmount < item.cost)
        {
            Debug.Log("Cannot buy");
        } else
        {
            playerData.items.Add(item);
            BonfireBehavior.OnAddItem?.Invoke(item);
            DisableButtons();
            OnContinue?.Invoke();
        }
    }

    public void EnableButtons()
    {
        healthIncreaseButton.gameObject.SetActive(true);
        jumpHeightIncreaseButton.gameObject.SetActive(true);
    }

    public void DisableButtons()
    {
        healthIncreaseButton.gameObject.SetActive(false);
        jumpHeightIncreaseButton.gameObject.SetActive(false);
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
        PlayerBehavior.OnTrashChange += UpdateTrashAmount;
        BonfireBehavior.OnRest += EnableButtons;

        ResetHUD();
        healthIncreaseButton.onClick.AddListener(() => AddHealthItem());
        jumpHeightIncreaseButton.onClick.AddListener(() => AddJumpHeightItem());
        resetButton.onClick.AddListener(() => ResetGame());
    }

    void OnDisable()
    {
        HealthComponent.OnDie -= PlayerDied;
        HealthComponent.OnAdjustHealth -= UpdateHealth;
        HealthComponent.OnAdjustMaxHealth -= UpdateHealth;
        PlayerBehavior.OnTrashChange -= UpdateTrashAmount;
        BonfireBehavior.OnRest -= EnableButtons;
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
        UpdateHealth();
        resetButton.gameObject.SetActive(true);
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
    }
}
