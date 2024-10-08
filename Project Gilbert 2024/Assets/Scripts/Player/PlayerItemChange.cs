using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerItemChange : MonoBehaviour
{
    public PlayerData playerData;

    public static Action OnTrashChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        BonfireBehavior.OnAddItem += AdjustStat;
    }

    private void OnDisable()
    {
        BonfireBehavior.OnAddItem -= AdjustStat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustStat(ItemData item)
    {
        var modifier = item.modifier;
        if (modifier != null) 
        {
            switch (modifier.stat) 
            {
                // Treat modifier value as a static integer increase instead of a percentage increase
                case Stats.Health:
                    GetComponent<HealthComponent>().AdjustMaxHealth((int)modifier.modifierValue);
                    break;
                case Stats.Stamina: 
                    playerData.maxStamina += (int)modifier.modifierValue;
                    break;
                case Stats.Damage: 
                    break;
                case Stats.MovementSpeed:
                    playerData.movementSpeed += playerData.movementSpeed * modifier.modifierValue;
                    break;
                case Stats.JumpHeight:
                    playerData.jumpForce += playerData.jumpForce * modifier.modifierValue;
                    break;
            }
        }
    }
}
