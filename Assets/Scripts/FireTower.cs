using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

/// <summary>
/// Script for each tower
/// </summary>
public class FireTower : MonoBehaviour
{
    
    [SerializeField] 
    private int cost = 0;

    private SpriteRenderer range;
    private bool isSelected = false;
    private Canvas canvas;
    private Animator anim;

    private const string animName = "Fire";
    private const string rangeObject = "Tower_01_Range_Green";

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        GameManager.Instance.Currency -= cost;
        
        range = gameObject.transform.Find(rangeObject).GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }

    void Update()
    {
        anim.SetTrigger(animName);
    }

    private void OnMouseDown()
    {
        if (!isSelected)
        {
            range.enabled = true;
            isSelected = true;
            canvas.enabled = true;
        }
        else
        {
            range.enabled = false;
            isSelected = false;
            canvas.enabled = false;
        }
    }

    public void SellTower()
    {
        GameManager.Instance.Currency += cost / 2;
        Destroy(gameObject);
    }
}
