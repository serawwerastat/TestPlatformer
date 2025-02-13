﻿using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float speed = 4f;

    public Transform target;

    void Start()
    {
        transform.position = new Vector3(target.transform.position.x,
            target.transform.position.y,
            transform.position.z);
    }

    void Update()
    {
        Vector3 position = target.position;
        position.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position,position, speed * Time.deltaTime);
    }
}