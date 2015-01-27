using UnityEngine;
using System.Collections;

public class LivingBeing : MonoBehaviour
{
    //Bools
    public bool isGrounded;

    //Floats
    public float moveSpeed = 1f;

    //Ints
    public int health = 100;
    public int direction = 1;
    //protected Animator anim;


    // Use this for initialization
    public virtual void Start()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckDeath();
        handleMovement();
    }

    public virtual void CheckDeath()
    {
        if (health <= 0)
        {
            //anim.SetBool("IsDead", true);
            Destroy(this);
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
    }

    public virtual void handleMovement() {
        Debug.Log("handle movement");
        if (isGrounded) {
            Debug.Log("Testing movement");
            rigidbody2D.velocity = new Vector2(moveSpeed * direction, 0);
        }
    }

    //Turn the being around
    public virtual void Flip()
    {
        //Multiply the x component of localScale by -1.
        Vector3 citizenScale = transform.localScale;
        citizenScale.x *= -1;
        transform.localScale = citizenScale;
        direction *= -1;
    }

    public virtual void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ground") {
            Debug.Log("Grounded");
            isGrounded = true;
        }
        if (col.gameObject.tag == "EdgeOfMap") {
            Flip();
        }
    }
}