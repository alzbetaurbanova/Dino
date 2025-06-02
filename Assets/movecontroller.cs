#pragma warning disable 0618
using UnityEngine;

public class Movecontroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintMulti;

    private float xInput;

    [Header("Collision check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;
    private bool facingRight = false;
    public bool isOnMovingPlatform = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Flip();
    }

    private void Update()
    {
        if (isOnMovingPlatform)
        {
            // Ak je hráč na pohybujúcej sa platforme, neposúvaj ho samostatne
            return;
        }

        AnimationControllers();
        xInput = Input.GetAxisRaw("Horizontal");
        Movement();
        CollisionChecks();
        FlipController();

        if (Input.GetKey(KeyCode.LeftShift))
            Sprint();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.F))
            Flip();
    }

    private void AnimationControllers()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void FlipController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x && facingRight)
            Flip();
        else if (mousePos.x > transform.position.x && !facingRight)
            Flip();
    }

    private void Sprint()
    {
        rb.velocity = new Vector2(xInput * moveSpeed * sprintMulti, rb.velocity.y);
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }


}
