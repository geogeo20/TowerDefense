using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    
    /// <summary>
    /// Script used on the projectiles to target the enemies
    /// </summary>
    private FollowPath target;

    private FireEnemy parent;
    private int damage;
    private float currentHp;
    private float maxHp;

    private int buffType;

    void Update()
    {
        moveToTarget();
        Debug.Log(damage);
    }
    public void Initialize(FireEnemy givenParent)
    {
        Debug.Log(givenParent);
        this.target = givenParent.Target;
        this.parent = givenParent;
        this.damage = givenParent.Damage;
        this.maxHp = target.Health[target.Type];
        this.currentHp = target.CurrentHealth;
        
    }
    private void moveToTarget()
    {
        if (target != null && target.enabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                Time.deltaTime * parent.ProjectileSpeed);
            Debug.Log(target+ "MovingTowardsTarget");
            
        }
        else if(target == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
            target.CurrentHealth -= damage;
            //30% chance to hit the debuff
            if(Random.Range(0,10) >= 2)
                Debuff();
            Debug.Log(target.CurrentHealth);
            target.SetHpBar();
        }
            
    }

    private void Debuff()
    {
        switch (this.tag)
        {
            case "Wood":
                buffType = 0;
                break;
            case "Earth":
                buffType = 1;
                break;
            case "Stone":
                buffType = 2;
                break;
            case "Fire":
                buffType = 3;
                break;
        }
        
        Debug.Log("Debuff type" + buffType + this.tag);

        if (buffType == 0)
        {
            target.Slow();
        }

        if (buffType == 1)
        {
            target.Stun();
        }

        if (buffType == 2)
        {
            target.CurrentHealth -= damage;
        }

        if (buffType == 3)
        {
           target.Fire();
        }

    }
    
}
