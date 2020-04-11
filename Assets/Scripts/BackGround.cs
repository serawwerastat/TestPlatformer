using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private float length, startPos;

    public new GameObject camera;

    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var cameraPosition = camera.transform.position;
        float temp = cameraPosition.x * (1 - parallaxEffect);
        float dist = cameraPosition.x * parallaxEffect;

        var position = transform.position;
        position = new Vector3(startPos + dist, position.y, position.z);
        transform.position = position;

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if(temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
