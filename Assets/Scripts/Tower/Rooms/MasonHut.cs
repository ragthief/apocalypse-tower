﻿using UnityEngine;
using System.Collections;

public class MasonHut : Commercial {
    public override void Initialise() {

    }

    public void Start() {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }
}
