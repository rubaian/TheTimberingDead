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

[Header("Sounds")]
[SerializeField] private AudioClip jumpSound; // Sound for jumping
[SerializeField] private AudioClip walkSound; // Sound for walking
private AudioSource audioSource; // AudioSource for playing sounds

private bool isWalking = false; // Track if the player is walking

void Start()
{
    anim = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

    // تأكد من أن AudioSource تم تعيينه
    if (audioSource == null)
    {
        Debug.LogError("AudioSource component is missing on this GameObject!");
    }

    // تأكد من أن الأصوات تم تعيينها
    if (jumpSound == null || walkSound == null)
    {
        Debug.LogWarning("Jump or Walk sound not assigned in the inspector!");
    }
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

    // Play walking sound if the player is moving on the ground
    if (IsGrounded() && horizontal != 0)
    {
        if (!isWalking)
        {
            isWalking = true;
            audioSource.clip = walkSound;
            audioSource.loop = true; // Loop the walk sound
            audioSource.Play();
        }
    }
    else if ((!IsGrounded() || horizontal == 0) && isWalking)
    {
        isWalking = false;
        audioSource.Stop(); // Stop the walking sound
    }
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

    // تأكد من أن الصوت لا يتكرر
    if (audioSource != null && jumpSound != null && !audioSource.isPlaying)
    {
        audioSource.PlayOneShot(jumpSound); // Play jump sound once
    }
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
        grounded = true;
}

// Method to check if the player can attack (only if not moving horizontally and grounded)
public bool CanAttack()
{
    return horizontal == 0 && IsGrounded();
}
}