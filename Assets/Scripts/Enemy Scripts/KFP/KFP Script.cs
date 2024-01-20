using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KFPScript : MonoBehaviour
{
    public EnemyType KFP;
    private float currentHealth;
    private float health;
    private float speed;
    private float damage;
    private float timeBtwAttacks;
    void Start()
    {
        health = KFP.enemyHealth;
        currentHealth = health;
        speed = KFP.enemySpeed;
        damage = KFP.enemyDamage;
        timeBtwAttacks = KFP.attackInterval;
    }

    void Update()
    {
        
    }
}
