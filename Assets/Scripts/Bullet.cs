using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;

    private float timeToDisable = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetDisabled());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(SetDisabled());
        gameObject.SetActive(false);
    }
}