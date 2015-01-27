using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Tower {

    static int currentPopulation = 0;
	static bool isOverPopulated = false;
    static bool canAddCitizen = false;

    private static List<Room> residentialRooms = new List<Room>();
    private static List<Room> commercialRooms = new List<Room>();
    private static List<Room> utilityRooms = new List<Room>();
    private static List<Human> totalPopulation = new List<Human>();

    private static bool keepAdding = false;

    public static void HouseResident(Human newResident)
    {
        keepAdding = true;
        //Finds an empty room for the resident to live.
        foreach (Residential room in residentialRooms)
        {
            if(room.getCurrentPopulation() < room.getPopulationCapacity() && keepAdding) {
                List<Human> addedCitizens = new List<Human>();
                addedCitizens.Add(newResident);
                room.AddResidents(addedCitizens);
                keepAdding = false;
            }
        }
    }

    public static void AddRoom(Room room)
    {
        room.gameObject.layer = 10;
        if (room is Residential)
            residentialRooms.Add(room);
        else if (room is Commercial)
            commercialRooms.Add(room);
        else
            utilityRooms.Add(room);

		TowerEvent.overPopulation.Update();
        World.ConstructGraph();
    }

    public static void DeleteRoom(Room room)
    {
        if (room is Residential)
            residentialRooms.Remove(room);
        else if (room is Commercial)
            commercialRooms.Remove(room);
        else
            utilityRooms.Remove(room);

		TowerEvent.overPopulation.Update();
        World.ConstructGraph();
    }

    public static int calculateRevenue() {
        int revenueCounter = 0;
        foreach (Commercial room in commercialRooms)
        {
            revenueCounter += room.getRevenue();
        }
        return revenueCounter;
    }

    public static int getPopulationCap()
    {
        int populationCap = 0;
        foreach (Residential room in residentialRooms)
        {
            populationCap += room.getPopulationCapacity();
        }
        return populationCap;
    }

    public static bool canAddCit()
    {
        if (getCurrentPopulation() < getPopulationCap())
            canAddCitizen = true;
        else canAddCitizen = false;
        return canAddCitizen;
    }

    public static int getCurrentPopulation()
    {
        //Allocate citizens to rooms later, use simpler line below for now.
        /*foreach (Residential room in residentialRooms)
        {
            currentPopulation = room.ListResidents.Count;
        }*/
        return currentPopulation;
    }

    public static void addCitizen(Human newResident)
    {
        //Find a residental building to house the spawned citizen in.
        HouseResident(newResident);      
        currentPopulation++;
		TowerEvent.overPopulation.Update();
    }

	public static bool IsOverPopulated
	{
		get { return isOverPopulated; }
		set { isOverPopulated = value; }
	}

    public static List<Room> ResidentialRoomList
    {
        get { return residentialRooms; }
    }

    public static List<Room> CommercialRoomList
    {
        get { return commercialRooms; }
    }

    public static List<Room> UtilityRoomList
    {
        get { return utilityRooms; }
    }
}
