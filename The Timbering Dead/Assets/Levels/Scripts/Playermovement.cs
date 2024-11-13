using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]private float speed;
    private Rigidbody2D body;
    private bool grounded;


   private void Awake() // Awake is called when the script instance is being loaded
   {
    body = GetComponent<Rigidbody2D>();
   }


   private void Update() // Update is called once per frame
   {

    float horizontalInput = Input.GetAxis("Horizontal");
    //Move
    body.velocity= new Vector2(Input.GetAxis("Horizontal")*speed,body.velocity.y);

    //Flip the player sprite
    if(horizontalInput > 0.01f)
    {
        transform.localScale = new Vector3(1,1,1);
    }
    else if(horizontalInput < 0.01f)
    {
        transform.localScale = new Vector3(-1,1,1);
    }
    
    //Jump
    if(Input.GetKey(KeyCode.Space) && grounded)
        Jump();
   }

   private void Jump()
   {
    body.velocity=new Vector2(body.velocity.x, speed);
    grounded = false;
   }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     if(collision.gameObject.tag == "Ground")
     {
          grounded = true;
     }
    }
}
