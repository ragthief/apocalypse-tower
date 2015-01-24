using UnityEngine;
using System.Collections;

public abstract class Commercial : Room {

    protected int commercialRevenue = 10;

    public virtual int getRevenue()
    {
        return commercialRevenue;
    }

    public override string displayInformation()
    {
        string displayString = "";
        displayString = "\n" + commercialRevenue.ToString();
        return displayString;
    }
}
