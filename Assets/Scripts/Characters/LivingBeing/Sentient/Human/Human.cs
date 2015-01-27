using UnityEngine;
using System.Collections;

public class Human : Sentient {
    
    // Use this for initialization
    public override void Start() {
        base.Start();
        Tower.addCitizen(this);
    }
}
