using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : Singleton<EnemySpawner>
{
    /// <summary>
    /// Script used for spawning monsters on the map
    /// </summary>
    
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameWinMenu;

    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private Text waveText;

    [SerializeField] private GameObject WaveButton;
    [SerializeField] private Text lifeText;


    private int enemyCount;

    public int EnemyCount
    {
        get => enemyCount;
        set => enemyCount = value;
    }

    private int wave;

    private float timer;

    private bool waveEnded = true;
    private GameObject lastEnemy = null;
    private bool isGameOver = false;

    private int lives;
    private bool lastWave = false;

    public int Lives
    {
        get => lives;
        set
        {
            this.lives = value;
            lifeText.text = lives.ToString();
            if (lives <= 0)
            {
                this.lives = 0;
                gameOver();
            }
        }
    }


    void Start()
    {
        timer = spawnRate;
        wave = 0;
        Lives = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWave == true && enemyCount == 0)
        {
            gameWin();
        }

    }

    public void SpawnEnemy()
    {
        if (waveEnded == true)
        {
            wave++;
            waveEnded = false;
            waveText.text = string.Format("Wave: <color=green>{0}</color>", wave);
            StartCoroutine(SpawnWave());
            
            
        }
        if (wave == 10)
        {
            lastWave = true;
        }

    }

    private IEnumerator SpawnWave()
    {
        GameObject currentEnemy;
        for (int i = 0; i < wave; i++)
        {
            currentEnemy = Instantiate(enemy);
            currentEnemy.name = $"Wave: {wave} - Count: {i+1}";
            if (i == wave - 1)
            {
                lastEnemy = enemy;
                waveEnded = true;
            }

            enemyCount++;

            yield return new WaitForSeconds(.5f);
        }
        

    }

    public void gameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void gameWin()
    {
        gameWinMenu.SetActive(true);
    }

    

    
}