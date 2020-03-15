using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using static DefaultNamespace.Constants;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LayerMask platformLayerMask;
    private Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    // public Transform groundCheck;
    private bool _isGrounded;
    private int _currentHp;
    public int maxHP = 3;
    private bool _isHit;
    public bool isImmune = false;
    public Main main;
    public bool key = false;
    public bool canTP = true;
    public bool inWater = false;
    private bool _isClimbing = false;
    private int _coins = 0;
    public GameObject blueGem, greenGem;
    private int _gemCount = 0;
    private float _hitTimer = 0f;
    public Image playerCountDown;
    private float _healthCountdownTimer = -1f;
    public float healthCountdownTimerUp = 30f;
    public Image healthCountdown;
    public Inventory inventory;
    public SoundEffector soundEffector;
    private CapsuleCollider2D _capsuleCollider2D;
    private PlayerHelper _playerHelper;
    public Joystick joystick;
    

    private void Awake()
    {
        _capsuleCollider2D = transform.GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerHelper = GetComponent<PlayerHelper>();
        _currentHp = maxHP;
    }
    
    void Update()
    {
        //key control
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        if (_healthCountdownTimer >= 0f)
        {
            _healthCountdownTimer += Time.deltaTime;
            if (_healthCountdownTimer >= healthCountdownTimerUp)
            {
                _healthCountdownTimer = 0f;
                RecountHP(-1);
            }
            else
            {
                healthCountdown.fillAmount = 1 - (_healthCountdownTimer / healthCountdownTimerUp);
            }
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -20f)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
        if (inWater && !_isClimbing)
        {
            _playerHelper.PlaySwimmingAnimation();
            // _isGrounded = true;
            //joystick control
            // if (joystick.Horizontal >= 0.2f || joystick.Horizontal >= -0.2f)
            // {
            //     Flip();
            // }

            //key control
            if (Input.GetAxis("Horizontal") != 0)
            {
                Flip();
            }
        }
        else
        {
            IsGrounded();
            //joystick control
            // if (joystick.Horizontal < 0.2f && joystick.Horizontal > -0.2f && _isGrounded && !_isClimbing)
            // {
            //     _playerHelper.PlayStandingAnimation();
            // }
            //key control
            if (Input.GetAxis("Horizontal") == 0 && IsGrounded() && !_isClimbing)
            {
                _playerHelper.PlayStandingAnimation();
            }
            else
            {
                Flip();
                if (IsGrounded() && !_isClimbing)
                {
                    _playerHelper.PlayWalkAnimation();
                }
            }
        }
        //joystick control
        // if (joystick.Horizontal >= 0.2f)
        // {
        //     print("right");
        //     rb.velocity = new Vector2(speed, rb.velocity.y);
        // }
        // else if (joystick.Horizontal <= -0.2f)
        // {
        //     print("left");
        //     rb.velocity = new Vector2(-speed, rb.velocity.y);
        // }
        // else
        // {
        //     print("none");
        //     rb.velocity = new Vector2(0, rb.velocity.y);
        // }
        //key control
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

    }
    
    private bool IsGrounded()
    {
        float extraHeightText = 0.5f;
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(_capsuleCollider2D.bounds.center,
            _capsuleCollider2D.bounds.size, CapsuleDirection2D.Vertical ,0f, 
            Vector2.down, extraHeightText, platformLayerMask);
        _isGrounded = raycastHit.collider != null;
        return _isGrounded;
    }

    public void Jump()
    {
        if (IsGrounded() && !_isClimbing)
        {
            // rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            // _anim.SetInteger("State", 3);
            _playerHelper.PlayJumpAnimation();
            // rb.velocity = Vector2.up * jumpHeight; 
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
    }

    void Flip()
    {
        Quaternion lookRight = Quaternion.Euler(0, 0, 0);
        Quaternion lookLeft = Quaternion.Euler(0, 180, 0);
        //joystick control
        // if (joystick.Horizontal >= 0.2f)
        // {
        //     transform.localRotation = lookRight;
        // }
        //
        // if (joystick.Horizontal <= -0.2f)
        // {
        //     transform.localRotation = lookLeft;
        // }

        //key control
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = lookRight;
        }
        
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = lookLeft;
        }
    }

    // void CheckGround()
    // {
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
    //     _isGrounded = colliders.Length > 1;
    //     if (!_isGrounded && !_isClimbing)
    //     {
    //         _anim.SetInteger("State", 3);
    //     }
    // }

    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0 && !isImmune)
        {
            soundEffector.PlayHitSound();
            _currentHp += deltaHP;
            // StopCoroutine(OnHit());
            isImmune = true;
            _isHit = true;
            StartCoroutine(OnHit());
        }
        else if (deltaHP > 0)
        {
            soundEffector.PlayPowerUpSound();
            _currentHp += deltaHP;
        }

        if (_currentHp > maxHP)
        {
            _currentHp = maxHP;
        }

        if (_currentHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()
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

        if (Math.Abs(GetComponent<SpriteRenderer>().color.g - 1) < 0.01f)
        {
            if (!blueGem.active)
            {
                isImmune = false;
            }

            StopCoroutine(OnHit());
        }

        if (Math.Abs(GetComponent<SpriteRenderer>().color.g) < 0.01f)
        {
            _isHit = false;
        }

        yield return new WaitForSeconds(0.04f);
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
            if (other.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                canTP = false;
                other.gameObject.GetComponent<Door>().Teleport(gameObject);
                StartCoroutine(TPWait());
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
            if (_currentHp != maxHP)
            {
            Destroy(other.gameObject);
            RecountHP(1);
            soundEffector.PlayGemSound();
            }
        }

        if (other.gameObject.CompareTag(Mushroom))
        {
            Destroy(other.gameObject);
            RecountHP(-1);
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
        if (other.gameObject.CompareTag(Ladder))
        {
            _isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                _playerHelper.PlayLadderIdleAnimation();
            }
            else
            {
                _playerHelper.PlayLadderClimingAnimation();
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }
        }

        if (other.gameObject.CompareTag(Icy))
        {
            if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
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
                RecountHP(-1);
            }
            else
            {
                playerCountDown.fillAmount = 1 - (_hitTimer / 3f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Ladder))
        {
            _isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (other.gameObject.CompareTag(Icy))
        {
            if (rb.gravityScale == 7f)
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
        animator.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isJump", false);
    }

    IEnumerator TPWait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    IEnumerator NoHit()
    {
        _gemCount++;
        blueGem.SetActive(true);
        checkGems(blueGem);
        isImmune = true;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Fading(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        isImmune = false;
        _gemCount--;
        blueGem.SetActive(false);
        checkGems(greenGem);
    }

    IEnumerator SpeedBonus()
    {
        _gemCount++;
        greenGem.SetActive(true);
        checkGems(greenGem);
        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(9f);
        StartCoroutine(Fading(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(5f);
        speed = speed / 2;
        _gemCount--;
        greenGem.SetActive(false);
        checkGems(blueGem);
    }

    void checkGems(GameObject gem)
    {
        if (_gemCount == 1)
        {
            gem.transform.localPosition = new Vector3(0f, 0.6f, gem.transform.localPosition.z);
        }

        else
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, gem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, gem.transform.localPosition.z);
        }
    }

    IEnumerator Fading(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1, 1, 1, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
        {
            StartCoroutine(Fading(spr, time));
        }
    }

    public int GetCoins()
    {
        return _coins;
    }

    public int GetHP()
    {
        return _currentHp;
    }

    public void BlueGem()
    {
        StartCoroutine(NoHit());
        soundEffector.PlayPowerUpSound();
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
        soundEffector.PlayPowerUpSound();
    }
}