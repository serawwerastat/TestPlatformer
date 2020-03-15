using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.Constants;

public class GroundPatrol : MonoBehaviour
{
    [SerializeField]
    private LayerMask platformLayerMask;
    public float speed = 1.5f;
    public bool moveLeft = true;
    public Transform groundDetect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 0.5f, platformLayerMask);

        if (groundInfo.collider == true && groundInfo.collider.gameObject.CompareTag(Ground))
        {
            
        }
        else            
        {
            if (moveLeft == true)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                moveLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;
            }
        }
    }
}
