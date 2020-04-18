using UnityEngine;
using DefaultNamespace;

public class FlyingPlatform : MonoBehaviour
{
    public Transform[] points;

    public float speed = 1f;

    private int i = 1;

    void Start()
    {
        transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.Player))
        {
            var position = transform.position;
            float posX = position.x;
            float posY = position.y;
            position =
                Vector3.MoveTowards(position,
                    points[i].position,
                    speed * Time.deltaTime);
            transform.position = position;

            var playerPosition = other.gameObject.transform.position;
            playerPosition = 
                new Vector3(playerPosition.x + position.x - posX,
                playerPosition.y + position.y - posY, 
                playerPosition.z);
            other.gameObject.transform.position = playerPosition;
            if (transform.position == points[i].position)
            {
                if (i < points.Length - 1)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
        }
    }
}
