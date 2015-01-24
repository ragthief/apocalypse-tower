using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BusinessPlan
{
    static List<BusinessPlan> avaiableBusinessTypes = new List<BusinessPlan>();
    protected string name;
    protected Room owner;

    public static void AddNewBusinessType(BusinessPlan bp)
    {
        avaiableBusinessTypes.Add(bp);
    }

    public static List<BusinessPlan> AvailableBusinessTypes                             //List of available business types
    {
        get { 
            return avaiableBusinessTypes; 
        }
    }
}
