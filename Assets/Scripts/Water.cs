using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float timer = 0;

    private float timerHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            timer = 0;
            transform.localScale = new Vector3(-transform.localScale.x,1,1);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().inWater = true;
            // timerHit += Time.deltaTime;
            // if (timerHit >= 2)
            // {
            //     other.gameObject.GetComponent<Player>().RecountHP(-1);
            //     timerHit = 0;
            // }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().inWater = false;
            timerHit = 0;
        }
    }
}
