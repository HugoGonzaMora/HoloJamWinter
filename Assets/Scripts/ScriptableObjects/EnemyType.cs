using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies", fileName ="New Enemy")]
public class EnemyType : ScriptableObject
{
    public float enemyHealth;
    public float enemySpeed;
    public float enemyDamage;
    public float attackInterval;

    public int weight;

    public Sprite newSprite;

    [Range(0, 100)]
    public int seedsDropChance;

    public enum EnemiesType
    {
        Takodachi,
        Chumbud,
        Kfp,
        Deadbeats,
        Teamates
    }
}
