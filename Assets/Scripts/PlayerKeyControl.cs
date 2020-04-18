using System.Collections;
using UnityEngine;
using static DefaultNamespace.Constants;


public class PlayerKeyControl : MonoBehaviour
{
    private static readonly int IsJump = Animator.StringToHash("isJump");
    [SerializeField]
    private LayerMask platformLayerMask;
    private Rigidbody2D playerRigidbody2D;
    private PlayerHelper _playerHelper;
    public float speed = 7;
    public bool inWater;
    private bool _isClimbing;
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
        if (playerRigidbody2D.velocity.y > jumpHeight)
        {
            playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, jumpHeight/2f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        speed = transform.GetComponent<Player>().speed;
        
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
        float extraHeightText = 0.01f;
        var bounds = _capsuleCollider2D.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.center.y-0.5f);
        Vector2 size = new Vector2(bounds.size.x*0.8f, bounds.size.y);
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(origin, size, CapsuleDirection2D.Vertical,
            0f, Vector2.down, extraHeightText, platformLayerMask);
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
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Trampoline))
        {
            jumpHeight *= 3;
            StartCoroutine(TrampolineAnim(other.gameObject.GetComponentInParent<Animator>()));
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
    
    IEnumerator TrampolineAnim(Animator animator)
    {
        animator.SetBool(IsJump, true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool(IsJump, false);
        jumpHeight /= 3;
    }
}
