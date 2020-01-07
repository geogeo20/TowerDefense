using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Sprite[] enemiesSprite;
    private SpriteRenderer currentSprite;
    
    [SerializeField] private GameObject pathToFollow;
    private PathCreator currentPath;

    [SerializeField] private float[] speedModifier;
    

    public float[] SpeedModifier
    {
        get => speedModifier;
        set => speedModifier = value;
    }

    private float timeParam;

    [SerializeField] private float[] health;

    private int[] damage = new int[] {3, 2, 1};

    private int currentDamage;

    public float[] Health
    {
        get => health;
    }

    private float currentMaxHp;
    [SerializeField] private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    private bool coroutineAloud;

    private Vector2 newPos;
    private int segments;
    private int currentSegment;

    private bool isSlowed = false;
    private float slowTime = 2f;

    private bool isStuned = false;
    private float stunTime = 1f;
    private float debuffCounter = 0;

    private bool isOnFire = false;
    private int fireTimes = 0;
    private float fireTime = 1f;
    private float fireDamage = .5f;

    private int type;
    private float currentSpeed;

    public int Type => type;

    private void Start()
    {
        currentPath = pathToFollow.GetComponent<PathCreator>();
        coroutineAloud = true;
        timeParam = 0f;
        segments = currentPath.path.NumSegments;
        currentSegment = 0;
        currentSprite = GetComponent<SpriteRenderer>();
        setType(Random.Range(0,3));
    }

  

    private void Update()
    {
        if (coroutineAloud)
        {
            StartCoroutine(FollowRouteRoutine(currentSegment));
        }
        
        // Enemies runnig out of hp die
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.Currency++;
            EnemySpawner.Instance.EnemyCount--;
        }
        
        removeStunSlow();
        if (isOnFire)
        {
            OnFire();
        }
        
    }

    public void setType(int entype)
    {
        //setting the type of the enemy (heave, medium, light)
        
        currentMaxHp = health[entype];
        currentHealth = currentMaxHp;
        currentSpeed = speedModifier[entype];
        currentSprite.sprite = enemiesSprite[entype];
        currentDamage = damage[entype];
    }

    public void SetHpBar()
    {
        //setting the hp bar of each individual enemy
        
        GetComponentInChildren<HealthBar>().SetSize(currentHealth/currentMaxHp);
    }

    public void IncreaseHp()
    {
        //Increase enemies hp to raise the difficulty
        
        for (int i = 0; i < health.Length; i++)
        {
            health[i] += 5;
        }
    }
    
    private IEnumerator FollowRouteRoutine(int segmentFollow)
    {
        //Enemies following the bezier curve
        
        coroutineAloud = false;

        Vector2[] route = new[]
        {
            currentPath.path[segmentFollow * 3], currentPath.path[segmentFollow * 3 + 1],
            currentPath.path[segmentFollow * 3 + 2],
            currentPath.path[segmentFollow * 3 + 3]
        };


        while (timeParam < 1)
        {
            timeParam += Time.deltaTime * currentSpeed;

            newPos = Mathf.Pow(1 - timeParam, 3) * route[0] + 3 * Mathf.Pow(1 - timeParam, 2) * timeParam * route[1] +
                     3 * (1 - timeParam) * Mathf.Pow(timeParam, 2) * route[2] + Mathf.Pow(timeParam, 3) * route[3];

            transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }

        timeParam = 0;
        currentSegment++;

        coroutineAloud = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Enemies hitting the base
        
        if (other.tag == "ExitHouse")
        {
            Destroy(gameObject);
            EnemySpawner.Instance.Lives -= currentDamage;
        }
    }

    private void removeStunSlow()
    {
        //Removing the stun/slow after the respective debuff time has passed
        
        if (isStuned)
        {
            debuffCounter += Time.deltaTime;

            if (debuffCounter >= stunTime)
            {
                isStuned = false;
                currentSpeed = speedModifier[type];
                debuffCounter = 0;
            }
        }
        
        if (isSlowed)
        {
            debuffCounter += Time.deltaTime;

            if (debuffCounter >= slowTime)
            {
                isSlowed = false;
                currentSpeed = speedModifier[type];
                debuffCounter = 0;
            }
        }
        
    }

    public void Stun()
    {
        //stun debuff
        
        if (!isStuned)
        {
            isStuned = true;
            currentSpeed = 0f;
                
        }
    }

    public void Slow()
    {
        //slow debuff
        
        if (!isSlowed && !isStuned)
        {
            isSlowed = true;
            currentSpeed = speedModifier[type] / 2;

        }
    }

    public void Fire()
    {
        if (!isOnFire)
        {
            isOnFire = true;
        }
    }

    public void OnFire()
    {
        // Fire over time debuff
        
        if (isOnFire)
        {
            debuffCounter += Time.deltaTime;
            if (debuffCounter >= fireTime)
            {
                
                fireTimes++;
                CurrentHealth -= fireDamage;
                GetComponentInChildren<HealthBar>().SetSize(currentHealth/health[type]);
                debuffCounter = 0;
            }

            if (fireTimes == 5)
            {
                fireTimes = 0;
                isOnFire = false;
            }
        }
        
    }
    
    
    
}


