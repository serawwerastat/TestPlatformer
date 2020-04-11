using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;

    public Transform door;

    public Sprite mid, top;
    
    public void Unlock()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = mid;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = top;
    }

    public void Teleport(GameObject player)
    {
        var position = door.position;
        player.transform.position = new Vector3(position.x, position.y, player.transform.position.z);
    }
}
