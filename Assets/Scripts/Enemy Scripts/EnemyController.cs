using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public event Action<EnemyController> OnEnemyDeath;
    
    public EnemyType enemySO;
    public TowerType ameSO;

    public float speed;
    public float health;
    public float currentHealth;
    public float damage;
    public float attackInterval;

    public int weight;
    public int seedsDropChance;
    private int bulletsToStun;
    private int stunBulletsNow = 0;
    
    GameObject target;


    public AudioClip dmgSound;

    public GameObject seedssPrefab;

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
        seedsDropChance = enemySO.seedsDropChance;
        bulletsToStun = ameSO.attacksToStun;
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
            EnemyDeathAction();
        }

        if (stunBulletsNow >= bulletsToStun)
        {
            speed = 0f;
            stunBulletsNow = 0;
            Invoke("ResetEnemySpeed", ameSO.stunTime);
        }
    }

    #region TriggerCheck

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Tower")
            {
                isAttacking = true;
                isWalking = false;
                target = collision.gameObject;
                StartCoroutine(Attack());
            }

            if (collision.gameObject.tag == "ObjectToDefend")
            {
                GameManager.Instance.isGameEnd = true;
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

    #endregion
    
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

    #region GetDamage

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

    #endregion

    private void DropSeeds()
    {
        int random = Random.Range(0, 101);
        if (random <= seedsDropChance)
        {
            GameManager.Instance.seedsCnt += 1;
            GameManager.Instance.UpdateSeeds();
        }
    }

    private void EnemyDeathAction()
    {
        DropSeeds();
            
        currentHealth = health;

        NotifyEnemyDeath();
            
        WaveManager.Instance.AddEnemyToPool(this.gameObject);
    }
    
    private void NotifyEnemyDeath() => OnEnemyDeath?.Invoke(this);

    public void AddStunBullet() => stunBulletsNow++;

    private void ResetEnemySpeed() => speed = enemySO.enemySpeed;
}