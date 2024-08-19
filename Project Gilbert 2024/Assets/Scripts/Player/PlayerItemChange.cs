using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerItemChange : MonoBehaviour
{
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        BonfireBehavior.OnAddItem += AdjustStat;
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
                    GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>().AdjustMaxHealth((int)modifier.modifierValue);
                    break;
                case Stats.Stamina: 
                    break;
                case Stats.Damage: 
                    break;
                case Stats.MovementSpeed:
                    break;
                case Stats.JumpHeight:
                    playerData.jumpForce += playerData.jumpForce * modifier.modifierValue;
                    break;
            }
        }
    }
}
