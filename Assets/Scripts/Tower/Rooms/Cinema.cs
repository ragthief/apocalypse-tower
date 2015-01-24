using UnityEngine;
using System.Collections;

public class Cinema : Commercial {

    public override void Initialise()
    {

    }

    void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
