﻿using UnityEngine;
using System.Collections;

public class Stairs : Utility {

    public override void Initialise()
    {

    }

    void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
