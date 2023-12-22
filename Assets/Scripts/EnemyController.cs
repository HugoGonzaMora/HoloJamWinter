using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyType enemySO;

    public float speed;
    public float health;
    public float currentHealth;
    public float damage;
    public float attackInterval;
    GameObject target;
    public bool isAttacking;

    public AudioClip dmgSound;

    public GameObject pointsPrefab;

    [Header("Animation")]
    public bool isWalking;

    private void Start()
    {
        speed = enemySO.enemySpeed; 
        health = enemySO.enemyHealth; 
        currentHealth = enemySO.enemyHealth;
        damage = enemySO.enemyDamage;
        attackInterval = enemySO.attackInterval;
        currentHealth = health;
    }


}