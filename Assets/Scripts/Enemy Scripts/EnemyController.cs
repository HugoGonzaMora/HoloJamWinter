using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public event Action<EnemyController> OnEnemyDeath;
    
    public EnemyType enemySO;

    public float speed;
    public float health;
    public float currentHealth;
    public float damage;
    public float attackInterval;

    public int weight;
    
    GameObject target;


    public AudioClip dmgSound;

    public GameObject pointsPrefab;

    [Header("Animation")]
    public bool isAttacking;
    public bool isWalking;
    public bool isDed;

    private void Start()
    {
        speed = enemySO.enemySpeed; 
        health = enemySO.enemyHealth; 
        currentHealth = health;
        damage = enemySO.enemyDamage;
        attackInterval = enemySO.attackInterval;
        currentHealth = health;
        weight = enemySO.weight;
    }

    private void Update()
    {
        if(target == null)
        {
            isAttacking = false;
        }
        if(!isAttacking && !isDed)
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            currentHealth = health;
            
            NotifyEnemyDeath();
            
            WaveManager.Instance.AddEnemyToPool(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the enemy is collisioning with a tower
         if(collision.gameObject.tag == "Tower")
         {
             isAttacking = true;
             isWalking = false;
             target = collision.gameObject;
             StartCoroutine(Attack());
         }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tower")
        {
            isAttacking = true;
            isWalking = false;
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            target = null;
            isAttacking = false;
            isWalking = true;
        }
    }

    public IEnumerator Attack()
    {
        //Coroutine to attack the towers
        if(target != null)
        {
            target.GetComponent<GuraTowerScript>()?.GetDamage(damage);
            target.GetComponent<KiaraTowerScript>()?.GetDamage(damage);
            target.GetComponent<CalliTowerScript>()?.GetDamage(damage);
            target.GetComponent<AmeTowerScript>()?.GetDamage(damage);
            target.GetComponent<InaTowerScript>()?.GetDamage(damage);
        }
        yield return new WaitForSeconds(attackInterval);
        StartCoroutine(Attack());
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void GetReflectDamage(float damage)
    {
        int percentOfDamage = (int)((health * damage) / 100);
        currentHealth -= percentOfDamage;
    }
    
    public IEnumerator GetPassiveDamage(float damage)
    {
        int percentOfDamage = (int)((health * damage) / 100);
        for (int i = 0; i < 5; i++)
        {
            currentHealth -= percentOfDamage;
            //Debug.Log(currentHealth);
            yield return new WaitForSeconds(1f);
            if (currentHealth <= 0)
            {
                break;
            }
        }
    }

    private void NotifyEnemyDeath() => OnEnemyDeath?.Invoke(this);
}