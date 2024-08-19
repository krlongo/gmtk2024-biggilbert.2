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

    // Start is called before the first frame update
    void Start()
    {
        DisableButtons();
        UpdateHealth();
        UpdateTrashAmount();
        healthIncreaseButton.onClick.AddListener(() => AddHealthItem());
        jumpHeightIncreaseButton.onClick.AddListener(() => AddJumpHeightItem());
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

        playerData.items.Add(item);
        BonfireBehavior.OnAddItem(item);
        DisableButtons();
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
        
        playerData.items.Add(item);
        BonfireBehavior.OnAddItem(item);
        DisableButtons();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        HealthComponent.OnDie += PlayerDied;
        HealthComponent.OnAdjustHealth += UpdateHealth;
        HealthComponent.OnAdjustMaxHealth += UpdateHealth;
        PlayerBehavior.OnTrashChange += UpdateTrashAmount;
        BonfireBehavior.OnRest += EnableButtons;
    }

    private void OnDisable()
    {
        HealthComponent.OnDie -= PlayerDied;
        HealthComponent.OnAdjustHealth -= UpdateHealth;
        HealthComponent.OnAdjustMaxHealth -= UpdateHealth;
        PlayerBehavior.OnTrashChange -= UpdateTrashAmount;
        BonfireBehavior.OnRest -= EnableButtons;
    }

    private void UpdateHealth()
    {
        Debug.Log("UpdateHealth");
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
