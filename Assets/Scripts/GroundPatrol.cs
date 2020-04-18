using UnityEngine;
using static DefaultNamespace.Constants;

public class GroundPatrol : MonoBehaviour
{
    [SerializeField]
    private LayerMask platformLayerMask;
    public float speed = 1.5f;
    public bool moveLeft = true;
    public Transform groundDetect;
    
    void Update()
    {
        transform.Translate(Vector2.left * (speed * Time.deltaTime));
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 0.5f, platformLayerMask);

        if (groundInfo.collider == true && groundInfo.collider.gameObject.CompareTag(Ground))
        {
            
        }
        else            
        {
            if (moveLeft)
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
