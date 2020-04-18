using System.Collections;
using UnityEngine;

public class AdvancedAirPatrol : MonoBehaviour
{
    public Transform[] points;

    public float speed = 2f;

    public float waitTime = 2f;

    private bool _canGo = true;

    private int index = 1;

    void Start()
    {
        gameObject.transform.position = new Vector3(points[0].position.x,
            points[0].position.y,
            transform.position.z);
    }

    void Update()
    {
        if (_canGo)
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

                _canGo = false;
                StartCoroutine(Waiting());
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        _canGo = true;
    }
}
