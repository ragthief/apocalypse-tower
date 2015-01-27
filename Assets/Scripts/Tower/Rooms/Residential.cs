using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Residential : Room {

    protected List<Human> residents = new List<Human>();
    protected int currentPopulation = 0;
    protected int populationCapacity = 4;

    //For adding multiple residents at same time.
    public virtual void AddResidents(List<Human> addedResidents)
    {
        if (populationCapacity >= addedResidents.Count + residents.Count)
        {
            residents.AddRange(addedResidents);
            currentPopulation++;
        }
    }

    public virtual void RemoveResident(List<Human> removedCitizens)
    {
        foreach (Human r in removedCitizens)
        {
            residents.Remove(r);
            currentPopulation--;
        }
    }

    public virtual int getCurrentPopulation()
    {
        return currentPopulation;
    }
    
    public virtual int getPopulationCapacity()
    {
        return populationCapacity;
    }

    public virtual List<Human> ListResidents
    {
        get { return residents; }
    }
    
    public override string displayInformation() {
        string displayString = "";
        int numResident = 1;
        foreach (Human resident in residents)
        {
            displayString += "\n" + "Resident #" + numResident + ": " + resident.getName();
            numResident++;
        }
        displayString += "\nLeft: " + this.getRoomLeft().ToString() + "\nRight: " + this.getRoomRight().ToString();
        return displayString;
    }
}
