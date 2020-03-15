using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Main main;
    public Sprite finishSprite;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = finishSprite;
            main.Win();
        }
    }
}
