using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;

    private float timeToDisable = 10f;

    void Start()
    {
        StartCoroutine(SetDisabled());
    }

    void Update()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
    }

    private IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
    }
}