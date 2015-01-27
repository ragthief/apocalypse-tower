using UnityEngine;
using System.Collections;

public class Refugee : Human {
    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void CheckDeath() {
        base.CheckDeath();
    }

    public override void TakeDamage(int damageAmount) {
        base.TakeDamage(damageAmount);
    }

    public override void handleMovement() {
        base.handleMovement();
    }

    //Turn the being around
    public override void Flip() {
        base.Flip();
    }

    public override void OnCollisionEnter2D(Collision2D col) {
        base.OnCollisionEnter2D(col);
    }
}
