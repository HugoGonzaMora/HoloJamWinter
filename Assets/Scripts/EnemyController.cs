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
        //currentHealth = enemySO.enemyHealth;
        damage = enemySO.enemyDamage;
        attackInterval = enemySO.attackInterval;
        currentHealth = health;
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
         
         else if(collision.gameObject.GetComponent<GuraTowerScript>() && GetComponent<CalliTowerScript>() &&
                 GetComponent<InaTowerScript>() && GetComponent<AmeTowerScript>() && GetComponent<KiaraTowerScript>() != null)
         {
             isAttacking = true;
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
        else if(collision.gameObject.GetComponent<GuraTowerScript>() && GetComponent<CalliTowerScript>() &&
                GetComponent<InaTowerScript>() && GetComponent<AmeTowerScript>() && GetComponent<KiaraTowerScript>() != null)
        {
            isAttacking = true;
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
}