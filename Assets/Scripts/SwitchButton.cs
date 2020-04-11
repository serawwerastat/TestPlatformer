using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.Constants;

public class SwitchButton : MonoBehaviour
{
    public GameObject[] block;

    public Sprite buttonOn;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(KeyBox))
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
