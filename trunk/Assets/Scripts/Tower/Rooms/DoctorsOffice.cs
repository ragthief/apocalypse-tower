using UnityEngine;
using System.Collections;

public class DoctorsOffice : Commercial {
    public override void Initialise()
    {

    }

    void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
