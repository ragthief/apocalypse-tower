using UnityEngine;
using System;
using System.Collections;

public class LivingBeing : MonoBehaviour
{
    //Constants
    public const int right = 1;
    public const int left = -1;

    //Floats
    protected float moveSpeed = 1f;
    protected float verticalMoveSpeed = 1f;

    //Ints
    protected int health = 100;
    protected int roamDirection;

    //protected Animator anim;


    // Use this for initialization
    public virtual void Start()
    {
        roamDirection = right;
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckDeath();
    }

    public virtual void FixedUpdate() {
        
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

    public virtual void HandleMovement() {
        rigidbody2D.velocity = Vector2.right * roamDirection;
    }

    //Turn the being around
    public virtual void Flip()
    {
        //Multiply the x component of localScale by -1.
        Vector3 citizenScale = transform.localScale;
        citizenScale.x *= -1;
        transform.localScale = citizenScale;
        roamDirection *= -1;
    }

    //The direction you want the being to face in is specified.
    public virtual void FlipTo(int direction) {
        Vector3 citizenScale = transform.localScale;
        if (direction == right) {
            citizenScale.x = Mathf.Abs(citizenScale.x);
            roamDirection = right;
        } else {
            citizenScale.x = Mathf.Abs(citizenScale.x) * -1;
            roamDirection = left;
        }
        transform.localScale = citizenScale;
    }

    public virtual void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "EdgeOfMap" || col.gameObject.tag == "LeftBuildingEdge" || col.gameObject.tag == "RightBuildingEdge") {
            Flip();
        }
    }
    public virtual void OnCollisionExit2D(Collision2D col) {

    }
    public virtual void OnTriggerEnter2D(Collider2D trigger) {

    }
}