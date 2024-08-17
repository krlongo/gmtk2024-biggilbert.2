using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stages
{
    Forest,
    Ice,
    Lava,
    None
}

[CreateAssetMenu]
public class AvalancheData : ScriptableObject
{
    public float MoveSpeed;
    public int MoveDirection;
    public Stages stage;
}
