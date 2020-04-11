using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touches : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;
            if (touchPos.x > Camera.main.transform.position.x)
            {
                var position = transform.position;
                position = new Vector3(position.x + 3f,
                    position.y,
                    position.z);
                transform.position = position;
            }
            else
            {
                var position = transform.position;
                position = new Vector3(position.x - 3f,
                    position.y,
                    position.z);
                transform.position = position;
            }

        }
    }
}
