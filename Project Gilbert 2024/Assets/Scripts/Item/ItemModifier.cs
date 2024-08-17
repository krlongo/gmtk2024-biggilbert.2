using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats 
{
    Damage,
    Health,
    Stamina,
    JumpHeight,
    MovementSpeed,
    None
}

[CreateAssetMenu]
public class ItemModifier : ScriptableObject
{
    public Stats stat;
    public float modifierValue;

}
