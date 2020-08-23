using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    [SerializeField]
    private GameObject[] ranges = null;

    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseFollow();
    }

    private void mouseFollow()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void setSprite(Sprite towerButton)
    {
        sprite.sprite = towerButton;
        sprite.enabled = true;
        switchRange(false);

    }

    public void switchRange(bool inRage)
    {
        Debug.Log("Color switched");
        if (inRage)
        {
            ranges[0].SetActive(true);
            ranges[1].SetActive(false);
            
        }
        else
        {
            ranges[0].SetActive(false);
            ranges[1].SetActive(true);
        }
    }
    

    public void removeSprite()
    {
        sprite.sprite = null;
        

        for (int i = 0; i < ranges.Length; i++)
        {
            ranges[i].SetActive(false);
        }
    }
    
    
}
