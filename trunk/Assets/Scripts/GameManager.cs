using UnityEngine;
using System.Collections;

public class GameManager: MonoBehaviour
{
    public AudioSource[] sounds;
    public AudioSource noise1;
    public AudioSource noise2;

    public static float dollars;
    public static int happiness = 100;
    public static int population = 0;
    //public GUISkin theSkin;                   //Removing eventually to draw with gameGUI.cs instead

    void Start()
    {
        //sounds = GetComponents<AudioSource>();
        //noise1 = sounds[0];
        //noise2 = sounds[0];
    }

    /*public static void getRevenue()
    {
        //Debug.Log("Todays revenue is " + Tower.calculateRevenue());
    }

    public static void getCurrentPopulation()
    {
        //Debug.Log("Current Population is " + Tower.getCurrentPopulation());
    }

    public static void getPopulationCapacity()
    {
        //Debug.Log("Population Capacity is " + Tower.getPopulationCap());
    }*/
}
