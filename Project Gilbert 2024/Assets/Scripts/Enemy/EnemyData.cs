using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int currentHealth;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public int stageLevel;
}
