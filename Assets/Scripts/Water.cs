using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

public class Water : MonoBehaviour
{
    private float timer = 0;

    private float timerHit;

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
        if (other.gameObject.CompareTag(Constants.Player))
        {
            other.GetComponent<PlayerKeyControl>().inWater = true;
            
            other.GetComponent<PlayerJoyStickControl>().inWater = true;
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
        if (other.gameObject.CompareTag(Constants.Player))
        {
            other.GetComponent<PlayerKeyControl>().inWater = false;
            other.GetComponent<PlayerJoyStickControl>().inWater = false;

            timerHit = 0;
        }
    }
}
