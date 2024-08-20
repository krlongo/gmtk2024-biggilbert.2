using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonfireHUD : MonoBehaviour
{
    public PlayerData playerData;

    public Button increaseHealth;
    public Button increaseStamina;
    public Button increaseSpeed;

    public Image goatSitting;
    public Image goatSleeping;
    public Image upgradeMenu;
    public Image backgroundImage;

    public float sleepingTimer;
    public bool isSleeping;

    public static Action OnUpgrade;
    public static Action OnContinue;

    void OnEnable()
    {
        goatSleeping.enabled = false;
        increaseHealth.onClick.AddListener(() => AddHealthItem());
        increaseStamina.onClick.AddListener(() => AddStaminaItem());
        increaseSpeed.onClick.AddListener(() => AddSpeedItem());
    }

    void OnDisable()
    {

    }

    public void AddHealthItem()
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        ItemModifier modifier = ScriptableObject.CreateInstance<ItemModifier>();
        modifier.stat = Stats.Health;
        modifier.modifierValue = 1;

        item.itemName = "Grass Backpack";
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
            OnUpgrade?.Invoke();
        }
    }

    public void AddStaminaItem()
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        ItemModifier modifier = ScriptableObject.CreateInstance<ItemModifier>();
        modifier.stat = Stats.JumpHeight;
        modifier.modifierValue = 1;

        item.itemName = "Energy Drink";
        item.description = "Stamina Up";
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
            OnUpgrade?.Invoke();
        }
    }

    public void AddSpeedItem()
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        ItemModifier modifier = ScriptableObject.CreateInstance<ItemModifier>();
        modifier.stat = Stats.JumpHeight;
        modifier.modifierValue = .25f;

        item.itemName = "Speedy Kicks";
        item.description = "Speed up";
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
            OnUpgrade?.Invoke();
        }
    }

    public void DisableButtons()
    {
        upgradeMenu.enabled = false;
        increaseHealth.gameObject.SetActive(false);
        increaseStamina.gameObject.SetActive(false);
        increaseSpeed.gameObject.SetActive(false);
        goatSitting.enabled = false;
        goatSleeping.enabled = true;
        isSleeping = true;
        sleepingTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleeping) return;
        if (sleepingTimer > 0)
        {
            sleepingTimer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("in Update");
            isSleeping = false;
            OnContinue?.Invoke();
        }
    }
}
