using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerType", menuName = "TowerType")]

public class TowerType : ScriptableObject
{
    [Header("Similar Variables")]
    public int health;
    public int cost;
    public int damage;
    
    public float timeBtwAtk;

    [Header("Kiara")] 
    public GameObject fireballPref;
    
    [Range(1, 15)]public int fireDamage;
    public int pointsProduct;
    
    [Range(10, 20)]public float timeBtwProductions;

    [Header("Calli")] 
    public int timeToDestroy;

    [Header("Ame")] 
    public GameObject bulletPref;
    public float decreaseSpeed;
    [Range(1, 5)]public float stunTime;

    public int attacksToStun;

    [Header("Gura")] 
    public GameObject waterballPref;
    public float distanceToMelee;
    
    public int meleeDamage;
    [Range(1, 7)]public int bleedingDamage;

    [Header("Ina")] 
    public GameObject tentaclePref;
    [Range(1, 10)] public int reflectedDamage;
}
