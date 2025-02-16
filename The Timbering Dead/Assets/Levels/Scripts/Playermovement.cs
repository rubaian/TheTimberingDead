using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpForce = 15f;
    private bool isFacingRight = true;
    private bool grounded;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // Reduce jump force
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();

        // Set the animator parameters
        anim.SetBool("walk", horizontal != 0);
        anim.SetBool("grounded", grounded);
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Flip the player
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Jump animation
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    // Method to check if the player can attack (only if not moving horizontally and grounded)
    public bool canAttack()
    {
        return horizontal == 0 && IsGrounded();
    }
}