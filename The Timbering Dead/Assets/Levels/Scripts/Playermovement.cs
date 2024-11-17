using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpForce = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

   void Start()
   {
       
   }

   void Update()
   {
       horizontal = Input.GetAxisRaw("Horizontal");
        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
         {
              rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpForce);
         }
        // Reduce jump force
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

       Flip();
   }

   private void FixedUpdate()
   {
    // Move the player
    rb.velocity = new UnityEngine.Vector2(horizontal * speed, rb.velocity.y);
   }

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
            UnityEngine.Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
       }
   }
}
