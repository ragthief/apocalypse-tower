using UnityEngine;
using System.Collections;

public class HotelDouble : Residential {
    public override void Initialise()
    {

    }

    public void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
