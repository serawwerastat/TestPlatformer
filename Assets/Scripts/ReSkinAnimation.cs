using System.Linq;
using UnityEngine;
using static DefaultNamespace.Constants;

public class ReSkinAnimation : MonoBehaviour
{
    string spriteSheetName;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(SkinActive))
        {
            spriteSheetName = PlayerPrefs.GetString(SkinActive);
        }
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        string path = "Characters/128x256/" + spriteSheetName + "/";
        Sprite[] allSubSprites = Resources.LoadAll<Sprite>(path);
        
        Sprite overrideSprite = allSubSprites.FirstOrDefault(s => s.name.Equals(_spriteRenderer.sprite.name));
        if (overrideSprite)
        {
            _spriteRenderer.sprite = overrideSprite;
        }
    }
}
