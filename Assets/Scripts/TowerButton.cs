using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private int cost;

    public int Cost
    {
        get => cost;
        set => cost = value;
    }

    public GameObject Tower
    {
        get => tower;
        set => tower = value;
    }

    private void Start()
    {
        Debug.Log(Tower.name);
    }

    private void Update()
    {
        
    }
}
