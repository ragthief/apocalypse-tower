using UnityEngine;
using System.Collections;

public abstract class Utility : Room {
    public override string displayInformation()
    {
        string displayString = "";
        displayString = "\n" + "Hey kid, stop all the downloading";
        return displayString;
    }

}
