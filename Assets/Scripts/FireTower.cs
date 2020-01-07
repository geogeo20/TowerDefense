using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class FireTower : MonoBehaviour
{
    /// <summary>
    /// Script for each tower
    /// </summary>
    
    [SerializeField] private float speedModifier;
    private bool direction;

    [SerializeField] private int cost;

    private GameObject range;
    private bool isSelected = false;
    private Canvas canvas;
    
    private Animator anim;
    void Start()
    {
        
        anim = GetComponentInChildren<Animator>();
        
        //Removing money after it was palced
        GameManager.Instance.Currency -= cost;
        
        //Getting the tower range visuals
        range = gameObject.transform.Find("Tower_01_Range_Green").gameObject;
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }

    private void OnDestroy()
    {
        //Selling the tower returns half of the cost
        GameManager.Instance.Currency += cost / 2;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetTrigger("Fire");
    }

    private void OnMouseDown()
    {
        //Clicking the tower will bring up the range and sell button
        
        if (!isSelected)
        {
            range.GetComponent<SpriteRenderer>().enabled = true;
            isSelected = !isSelected;
            canvas.enabled = true;
        }
        else
        {
            range.GetComponent<SpriteRenderer>().enabled = false;
            isSelected = !isSelected;
            canvas.enabled = false;
        }
        
    }

    public void SellTower()
    {
        Destroy(gameObject);
    }
}
