using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.Constants;


public class PlayerKeyControl : MonoBehaviour
{
    [SerializeField]
    private LayerMask platformLayerMask;
    private Rigidbody2D playerRigidbody2D;
    private PlayerHelper _playerHelper;
    public float speed = 7;
    public bool inWater = false;
    private bool _isClimbing = false;
    private bool _isGrounded;
    private CapsuleCollider2D _capsuleCollider2D;
    private SoundEffector soundEffector;
    public float jumpHeight = 12f;

    private void Awake()
    {
        _capsuleCollider2D = transform.GetComponent<CapsuleCollider2D>();
    }
    
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerHelper = GetComponent<PlayerHelper>();
        soundEffector = GetComponent<Player>().soundEffector;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (inWater && !_isClimbing)
        {
            _playerHelper.PlaySwimmingAnimation();
            if (Input.GetAxis("Horizontal") != 0)
            {
                Flip();
            }
        }
        else
        {
            IsGrounded();
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
        playerRigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerRigidbody2D.velocity.y);
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
            _playerHelper.PlayJumpAnimation();
            playerRigidbody2D.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
    }
    
    void Flip()
    {
        Quaternion lookRight = Quaternion.Euler(0, 0, 0);
        Quaternion lookLeft = Quaternion.Euler(0, 180, 0);

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = lookRight;
        }
        
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = lookLeft;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Ladder))
        {
            _isClimbing = true;
            playerRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                _playerHelper.PlayLadderIdleAnimation();
            }
            else
            {
                _playerHelper.PlayLadderClimingAnimation();
                transform.Translate(Vector3.up * (Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Ladder))
        {
            _isClimbing = false;
            playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
