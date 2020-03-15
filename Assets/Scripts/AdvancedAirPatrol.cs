﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAirPatrol : MonoBehaviour
{
    public Transform[] points;

    public float speed = 2f;

    public float waitTime = 2f;

    private bool CanGo = true;

    private int index = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(points[0].position.x,
            points[0].position.y,
            transform.position.z);
    }

    void Update()
    {
        if (CanGo)
        {
            transform.position =
                Vector3.MoveTowards(transform.position,
                    points[index].position,
                    speed * Time.deltaTime);

            if (transform.position == points[index].position)
            {
                if (index < points.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }

                CanGo = false;
                StartCoroutine(Waiting());
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        CanGo = true;
    }
}
