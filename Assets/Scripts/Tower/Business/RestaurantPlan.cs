using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestaurantPlan : BusinessPlan
{
    string[] allMenuItems = { "Chicken", "Cheese", "Chutney" }; // Such a selection
    List<string> currentMenuItems = new List<string>();

    public RestaurantPlan(string name, Room owner)
    {
        this.name = name;
        this.owner = owner;
    }

    public void AddMenuItem(int index)
    {
        currentMenuItems.Add(allMenuItems[index]);
    }

    public void RemoveMenuItem(string name)
    {
        currentMenuItems.Remove(currentMenuItems.Find(o => o == name));
    }

    public string[] AllMenuItems
    {
        get { return allMenuItems; }
    }

    public List<string> CurrentMenuItems
    {
        get { return currentMenuItems; }
    }
}

