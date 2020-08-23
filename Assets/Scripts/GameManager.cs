using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Script for different purposes all over the game
    /// </summary>


    [SerializeField]
    private GameObject menu = null;    

    public TowerButton ClickedButton { get; private set; }

    private bool towerPicked = false;

    [SerializeField] 
    private Text currencyTxt = null;
    [FormerlySerializedAs("currency")] [SerializeField] private int currency;

    public int Currency
    {
        get => currency;
        set
        {
            currency = value;
            currencyTxt.text = value.ToString() + "<color=green>$</color>";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Currency = 15;
    }

    // Update is called once per frame
    void Update()
    {
        //Removing the selected tower from the mouse
        if (Input.GetKeyDown(KeyCode.Escape) && towerPicked)
        {
            Hover.Instance.removeSprite();
            releaseButton();
        }

        
        //Bring up pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void PickTower(TowerButton towerButton)
    {
        //Picking the tower if enough money is available
        
        if (towerButton.Cost <= Currency)
        {
            ClickedButton = towerButton;
            towerPicked = true;
        }
    }

    public void SetTowerSprite(Sprite towerSprite)
    {
        //set the tower sprite when moving the mouse after picking the tower
        if (ClickedButton.Cost <= Currency)
        {
            Hover.Instance.setSprite(towerSprite);
        }
       
    }

    public void releaseButton()
    {
        ClickedButton = null;
        towerPicked = false;
    }

    public bool isTowerPicked()
    {
        return towerPicked;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
    }

    public void restartScene()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene("MainScene");
    }
    
    public void quitGame()
    {
        Application.Quit();
    }
    
    
    
}
