using System.Collections;
using UnityEngine;

public class AirPatrole : MonoBehaviour
{
    public Transform point1;

    public Transform point2;

    public float speed = 2f;

    public float waitTime = 1f;

    private bool _canGo = true;
    
    void Start()
    {
        var position = point1.position;
        gameObject.transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    void Update()
    {
        if (_canGo)
        {
            transform.position =
                Vector3.MoveTowards(transform.position,
                    point1.position,
                    speed * Time.deltaTime);

            if (transform.position == point1.position)
            {
                Transform t = point1;
                point1 = point2;
                point2 = t;
                _canGo = false;
                StartCoroutine(Waiting());
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        if (transform.rotation.y == 0)
        {
            transform.eulerAngles = new Vector3(0,  180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        _canGo = true;
    }
}