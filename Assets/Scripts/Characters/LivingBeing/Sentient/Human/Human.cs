using UnityEngine;
using System.Collections;

public class Human : Sentient {
    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public virtual void CheckDeath() {
        base.CheckDeath();
    }

    public virtual void TakeDamage(int damageAmount) {
        base.TakeDamage(damageAmount);
    }

    public virtual void handleMovement() {
        base.handleMovement();
    }

    //Turn the being around
    public virtual void Flip() {
        base.Flip();
    }

    public override void OnCollisionEnter2D(Collision2D col) {
        base.OnCollisionEnter2D(col);
    }
}
