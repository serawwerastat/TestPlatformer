using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCarpet : MonoBehaviour
{
    public Transform left;

    public Transform right;

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            RaycastHit2D leftWall = Physics2D.Raycast(left.position, Vector2.left, 0.5f);
            RaycastHit2D rightWall = Physics2D.Raycast(right.position, Vector2.right, 0.5f);
            if ((Input.GetAxis("Horizontal") > 0
                 && !rightWall.collider
                 && other.transform.position.x > transform.position.x)
                || (Input.GetAxis("Horizontal") < 0
                    && !leftWall.collider
                    && other.transform.position.x < transform.position.x))
            {
                transform.position =
                    new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }
}