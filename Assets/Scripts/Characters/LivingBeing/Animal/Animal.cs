using UnityEngine;
using System.Collections;

public class Animal : LivingBeing {
    public float moveSpeed = 2f;
    public bool isHungry = true;
    public bool isFood = true;
    public int trackHunger = 100;

    // Use this for initialization
    public virtual void Start() {
        InitialiseAnimal();
    }

    // Update is called once per frame
    public virtual void Update() {
        CheckDeath();
        UpdateMovement();
    }

    public virtual void InitialiseAnimal() {
        //anim = GetComponent<Animator>();
        //Debug.Log(anim.ToString());
    }

    public virtual void UpdateMovement() {
        Debug.Log("IsHungry: " + isHungry.ToString() + "\n" + "isFood: " + isFood.ToString());
        if (isHungry && isFood) {
            Debug.Log("Eating");
            rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
            //anim.SetBool("IsHungry", isHungry);
            //anim.SetBool("OnFood", isFood);
            isHungry = false;
            trackHunger = 100;
        }

        //Else move around normally
        else {
            //anim.SetFloat("Speed", moveSpeed);
            //Set the character's velocity to moveSpeed in the x direction.
            rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
            trackHunger--;
            if (trackHunger < 20) {
                isHungry = true;

            }
            Debug.Log("Track hunger is: " + trackHunger.ToString());
        }
    }

    public override void CheckDeath() {
        base.CheckDeath();
    }

    public override void TakeDamage(int damageAmount) {
        base.TakeDamage(damageAmount);
    }

    public virtual void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "food") {
            isFood = true;
        }
        if (col.gameObject.tag == "EdgeOfMap") {
            Flip();
        }
    }

    public virtual void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "food") {
            isFood = false;
        }
    }

    public override void Flip() {
        base.Flip();
    }
}