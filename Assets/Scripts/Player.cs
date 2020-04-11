using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using static DefaultNamespace.Constants;

public class Player : MonoBehaviour
{
    [SerializeField] private float deathFallHeight = -20f;
    private Rigidbody2D rb;
    public float speed;
    private int _currentHp;
    public int maxHp = 3;
    private volatile bool _isHit;
    private volatile bool isImmune;
    public Main main;
    public bool key;
    public bool canTp = true;
    private int _coins;
    public GameObject blueGem, greenGem;
    private int _gemCount;
    private float _hitTimer;
    public Image playerCountDown;
    private float _healthCountdownTimer = -1f;
    public float healthCountdownTimerUp = 30f;
    public Image healthCountdown;
    public Inventory inventory;
    public SoundEffector soundEffector;
    public Joystick joystick;
    private static readonly int IsJump = Animator.StringToHash("isJump");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _currentHp = maxHp;
    }
    
    void Update()
    {
        if (_healthCountdownTimer >= 0f)
        {
            _healthCountdownTimer += Time.deltaTime;
            if (_healthCountdownTimer >= healthCountdownTimerUp)
            {
                _healthCountdownTimer = 0f;
                RecountHp(-1);
            }
            else
            {
                healthCountdown.fillAmount = 1 - (_healthCountdownTimer / healthCountdownTimerUp);
            }
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < deathFallHeight)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    
    public void RecountHp(int deltaHp)
    {
        if (deltaHp < 0)
        {
            soundEffector.PlayHitSound();
            _currentHp += deltaHp;
            StopCoroutine(OnHit());
            _isHit = true;
            StartCoroutine(OnHit());
        }
        else if (deltaHp > 0)
        {
            soundEffector.PlayPowerUpSound();
            _currentHp += deltaHp;
        }

        if (_currentHp > maxHp)
        {
            _currentHp = maxHp;
        }

        if (_currentHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    private IEnumerator OnHit()
    {
        float changeColorSpeed = 0.04f;
        if (_isHit)
        {
            GetComponent<SpriteRenderer>().color =
                new Color(1,
                    GetComponent<SpriteRenderer>().color.g - changeColorSpeed,
                    GetComponent<SpriteRenderer>().color.b - changeColorSpeed);
        }
        else
        {
            GetComponent<SpriteRenderer>().color =
                new Color(1,
                    GetComponent<SpriteRenderer>().color.g + changeColorSpeed,
                    GetComponent<SpriteRenderer>().color.b + changeColorSpeed);
        }

        if (GetComponent<SpriteRenderer>().color.g >= 0.99f)
        {
            StopCoroutine(OnHit());
        }

        if (GetComponent<SpriteRenderer>().color.g <= 0.01f)
        {
            _isHit = false;
        }

        yield return new WaitForSeconds(changeColorSpeed);
        StartCoroutine(OnHit());
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Key))
        {
            Destroy(other.gameObject);
            key = true;
            inventory.AddKey();
            soundEffector.PlayGemSound();
        }

        if (other.gameObject.CompareTag(Constants.Door))
        {
            if (other.gameObject.GetComponent<Door>().isOpen && canTp)
            {
                canTp = false;
                other.gameObject.GetComponent<Door>().Teleport(gameObject);
                StartCoroutine(TeleportWait());
            }
            else if (key)
            {
                other.gameObject.GetComponent<Door>().Unlock();
            }
        }

        if (other.gameObject.CompareTag(Coin))
        {
            Destroy(other.gameObject);
            _coins++;
            soundEffector.PlayCoinSound();
        }

        if (other.gameObject.CompareTag(Heart))
        {
            if (_currentHp != maxHp)
            {
            Destroy(other.gameObject);
            RecountHp(1);
            soundEffector.PlayGemSound();
            }
        }

        if (other.gameObject.CompareTag(Mushroom))
        {
            Destroy(other.gameObject);
            RecountHp(-1);
            soundEffector.PlayHitSound();
        }

        if (other.gameObject.CompareTag(GemBlue))
        {
            Destroy(other.gameObject);
            inventory.AddBlueGem();
            soundEffector.PlayGemSound();
        }

        if (other.gameObject.CompareTag(GemGreen))
        {
            Destroy(other.gameObject);
            inventory.AddGreenGem();
            soundEffector.PlayGemSound();
        }
        
        if (other.gameObject.CompareTag(GemYellow))
        {
            Destroy(other.gameObject);
            inventory.AddYellowGem();
            soundEffector.PlayGemSound();
        }

        if (other.gameObject.CompareTag(TimerButtonStart))
        {
            _healthCountdownTimer = 0f;
        }

        if (other.gameObject.CompareTag(TimerButtonStop))
        {
            _healthCountdownTimer = -1f;
            healthCountdown.fillAmount = 0f;
        }

        if (other.gameObject.CompareTag(HiddenPass))
        {
            other.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.5f);
            SpriteRenderer[] spriteRenderers = other.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1,1,1, 0.5f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Icy))
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 15f;
                speed *= 0.25f;
            }
        }

        if (other.gameObject.CompareTag(Lava))
        {
            _hitTimer += Time.deltaTime;
            if (_hitTimer >= 3f)
            {
                _hitTimer = 0;
                playerCountDown.fillAmount = 1;
                RecountHp(-1);
            }
            else
            {
                playerCountDown.fillAmount = 1 - (_hitTimer / 3f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag(Icy))
        {
            if (rb.gravityScale == 15f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }

        if (other.gameObject.CompareTag(Lava))
        {
            _hitTimer = 0f;
            playerCountDown.fillAmount = 0f;
        }
        
        if (other.gameObject.CompareTag(HiddenPass))
        {
            other.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1);
            SpriteRenderer[] spriteRenderers = other.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1,1,1, 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Trampoline))
        {
            StartCoroutine(TrampolineAnim(other.gameObject.GetComponentInParent<Animator>()));
        }

        if (other.gameObject.CompareTag(QuickSand))
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(QuickSand))
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    IEnumerator TrampolineAnim(Animator animator)
    {
        animator.SetBool(IsJump, true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool(IsJump, false);
    }

    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(1f);
        canTp = true;
    }
    
    public int GetCoins()
    {
        return _coins;
    }

    public int GetHp()
    {
        return _currentHp;
    }

    public bool GetImmune()
    {
        return isImmune;
    }

    public void ImmuneOn()
    {
        isImmune = true;
    }

    public void ImmuneOff()
    {
        Invoke(nameof(SetImmuneFalse), 2);
    }
    void SetImmuneFalse()
    {
        isImmune = false;
    }
}
