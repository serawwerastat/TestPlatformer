using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public GameObject[] block;

    public Sprite buttonOn;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "KeyBox")
        {
            GetComponent<SpriteRenderer>().sprite = buttonOn;
            GetComponent<CircleCollider2D>().enabled = false;
            foreach (GameObject o in block)
            {
                Destroy(o);
            }
        }
    }
}
