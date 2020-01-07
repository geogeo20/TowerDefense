using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : MonoBehaviour
{
    /// <summary>
    /// Script used to target and shot at enemies
    /// </summary>
    
    private FollowPath target; //enemies main script

    public FollowPath Target
    {
        get => target;
        set => target = value;
    }

    private Queue<FollowPath> enemies = new Queue<FollowPath>();

    private bool canAttack = true;
    private float attackTimer = 0f;
    
    [SerializeField]
    private float attackCooldown = 1f;

    [SerializeField] private GameObject projectileType;

    [SerializeField] private float projectileSpeed = 3f;

    private int damage = 5;

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public float ProjectileSpeed
    {
        get => projectileSpeed;
        set => projectileSpeed = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Damage);
        Atack();
        
    }

    private void Atack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        
        if (target == null && enemies.Count > 0)
        {
            target = enemies.Dequeue();
        }

        if (target != null && target.isActiveAndEnabled )
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
            
        }
    }

    private void Shoot()
    {
        //Shooting the active target
        
        Projectile projectile = projectileType.GetComponent<Projectile>();
        Projectile newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.Initialize(this);
        Debug.Log("bullet initiliazed");

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If more than one enemies is in range they'll get queued up and targeted later when the main target is out of range
        
        if (other.tag == "Enemy")
        {
            enemies.Enqueue(other.GetComponent<FollowPath>());
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = null;
        }
    }
}
