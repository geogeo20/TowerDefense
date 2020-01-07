using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MountTower : MonoBehaviour
{
    /// <summary>
    /// Script used on the mounting points for the towers
    /// </summary>
    
    [SerializeField] private GameObject[] towers;
    
    
    [SerializeField] private Vector3 offset = new Vector3(0f, .43f, 0f);

    private bool isMounted = false;
    private GameObject mountedObject;

    private SpriteRenderer point;
    
    // Start is called before the first frame update
    void Start()
    {
        point = GetComponent<SpriteRenderer>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
        //if the tower was sold it will be removed and the mouting point will reapper
        if (mountedObject == null && isMounted==true)
        {
            isMounted = !isMounted;
            removePoint();
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        
    }

    private void OnMouseDown()
    {
        //Placing the tower and removing the mounting point
        
        if (!isMounted)
        {
            PlaceTower();
            isMounted = !isMounted;
            removePoint();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        
    }
    
    private void PlaceTower()
    {
        mountedObject = Instantiate(GameManager.Instance.ClickedButton.Tower, transform.position + offset,
            transform.rotation);
        mountedObject .transform.SetParent(transform);
        
        //release the current tower from the mounting queue
        
        GameManager.Instance.releaseButton();
        
        //remove the hover over mouse effect
        Hover.Instance.removeSprite();
    }

    private void removePoint()
    {
        point.enabled = !point.enabled;
    }

    
    private void OnMouseEnter()
    {
        //Checking if the tower can be placed over the current mouse position
        
        if (GameManager.Instance.isTowerPicked())
        {
            Hover.Instance.switchRange(true);
        }
        Debug.Log("MouseOver");
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.isTowerPicked())
        {
            Hover.Instance.switchRange(false);
        }
        Debug.Log("MouseOut");
    }
}
