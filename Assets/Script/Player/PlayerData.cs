using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public PlayerStats playerStats;
    [Space(1)]
    public MovementData MovementData;
    [Space(1)]
    public CombatData CombatData;
}

[Serializable]
public class PlayerStats
{
    [Header("Player Stats")]
    public float MaxHealth;
    public float DefaultInvisibleCooldown;
    public float KnockbackMagnitude;
}