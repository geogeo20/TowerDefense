using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used for spawning monsters on the map
/// </summary>
public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField]
    private GameObject enemy = null;
    [SerializeField]
    private GameObject gameOverMenu = null;
    [SerializeField]
    private GameObject gameWinMenu = null;
    [SerializeField]
    private Text waveText = null;
    [SerializeField]
    private Text lifeText = null;

    private int enemyCount;
    private int wave;
    private bool waveEnded = true;
    private GameObject lastEnemy = null;
    private bool isGameOver = false;
    private int lives;
    private bool lastWave = false;



    void Start()
    {
        wave = 0;
        Lives = 15;
    }

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
            currentEnemy.name = $"Wave: {wave} - Count: {i + 1}";
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

    #region Properties
    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
        }
    }
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            lifeText.text = lives.ToString();
            if (lives <= 0)
            {
                lives = 0;
                gameOver();
            }
        }
    }

    #endregion
}