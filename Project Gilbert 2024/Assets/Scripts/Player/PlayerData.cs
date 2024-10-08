using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public int maxHealth;
    public int currentHealth;
    public Vector2 currentPosition;
    public float jumpForce;
    public float movementSpeed;
    public float climbingSpeed;
    public float maxStamina;
    public float currentStamina;
    public List<ItemData> items;
    public int trashAmount;
    public bool isDead;
    public bool isInvicible;
    public float invicibleTimer;
    public int currentLevel;
}
