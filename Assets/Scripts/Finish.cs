using UnityEngine;
using DefaultNamespace;

public class Finish : MonoBehaviour
{
    public Main main;
    public Sprite finishSprite;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.Player))
        {
            GetComponent<SpriteRenderer>().sprite = finishSprite;
            main.Win();
        }
    }
}
