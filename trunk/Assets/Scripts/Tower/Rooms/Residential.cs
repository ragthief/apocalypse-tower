using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Residential : Room {

    protected List<Citizen> residents = new List<Citizen>();
    protected int currentPopulation = 0;
    protected int populationCapacity = 4;

    //For adding multiple residents at same time.
    public virtual void AddResidents(List<Citizen> addedResidents)
    {
        if (populationCapacity >= addedResidents.Count + residents.Count)
        {
            residents.AddRange(addedResidents);
            currentPopulation++;
        }
    }

    public virtual void RemoveResident(List<Citizen> removedCitizens)
    {
        foreach (Citizen r in removedCitizens)
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

    public virtual List<Citizen> ListResidents
    {
        get { return residents; }
    }
    
    public override string displayInformation() {
        string displayString = "";
        int numResident = 1;
        foreach (Citizen resident in residents)
        {
            displayString += "\n" + "Resident #" + numResident + ": " + resident.getName();
            numResident++;
        }
        displayString += "\nLeft: " + this.getRoomLeft().ToString() + "\nRight: " + this.getRoomRight().ToString();
        return displayString;
    }
}
