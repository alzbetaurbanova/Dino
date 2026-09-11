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
    private Camera mainCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        // Without zero friction the player sticks to the side of a platform or cloud and hangs there.
        rb.sharedMaterial = new PhysicsMaterial2D("PlayerNoFriction") { friction = 0f, bounciness = 0f };

        // Moving platforms interpolate, so the player must too or it visually drifts ahead of them.
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        Flip();
    }

    private void Update()
    {
        CollisionChecks();

        AnimationControllers();
        xInput = Input.GetAxisRaw("Horizontal");
        Movement();
        
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
        {
            //Gizmos.color = Color.green; // farba gule
            //Gizmos.DrawSphere(groundCheck.position, groundCheckRadius); // plná guľa
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void FlipController()
    {
        if (mainCamera == null) return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
