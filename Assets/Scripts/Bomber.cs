using System.Collections;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject bullet;

    public Transform shoot;

    public float timeShoot = 2f;

    void Start()
    {
        var position = transform.position;
        shoot.transform.position = 
            new Vector3(position.x, position.y - 1f, position.z);
        StartCoroutine(Shooting());
    }
    
    private IEnumerator Shooting()
    {
        yield return  new WaitForSeconds(timeShoot);
        Instantiate(bullet,shoot.transform.position, transform.rotation);

        StartCoroutine(Shooting());
    }
}
