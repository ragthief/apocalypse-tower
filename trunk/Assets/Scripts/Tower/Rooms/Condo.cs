using UnityEngine;
using System.Collections;

public class Condo : Residential {
    public override void Initialise()
    {

    }

    void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
