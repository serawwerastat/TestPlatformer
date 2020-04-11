using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatle : MonoBehaviour
{
    public float speed = 4f;

    private bool isWait;

    public bool isHidden;

    public float waitTime = 3f;

    public Transform point;
    // Start is called before the first frame update
    void Start()
    {
        var position = transform.position;
        point.transform.position = 
            new Vector3(position.x, position.y + 1f, position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                point.position, 
                speed * Time.deltaTime);
        }

        if (transform.position == point.position)
        {
            if (isHidden)
            {
                var position = transform.position;
                point.transform.position = 
                    new Vector3(position.x, position.y + 1f, position.z);
                isHidden = false;
            }
            else
            {
                var position = transform.position;
                point.transform.position = 
                    new Vector3(position.x, position.y - 1f, position.z);
                isHidden = true;
            }

            isWait = true;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isWait = false;
    }
}
