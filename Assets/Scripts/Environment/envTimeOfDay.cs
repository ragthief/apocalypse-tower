using UnityEngine;
using System.Collections;

public class envTimeOfDay
{

    static float tBoost = 1; // Time Scale, 60 will be devided by this to make time faster. 1 = Normal!

    static float tUnitTime; // At a boost of 1, tUnitTime will make the in game clock run at 1 minute per 1 real life second.

    static float tCurSecond = 0;
    static int tCurMinute = 0;
    static int tCurHour = 0;
    static int tCurDay = 0;
    static int tCurMonth = 0;
    static int tCurYear = 0;

    static bool isPaused = false;

    // Will be called once every frame from the main script.
    public static void UpdateTime()
    {
        if (!isPaused)
        {
            tCurSecond += tUnitTime * Time.deltaTime;
            if (tCurSecond >= 60) { tCurSecond = 0; tCurMinute++; }
            if (tCurMinute >= 60) { tCurMinute = 0; tCurHour++; }
            if (tCurHour >= 24) {
                tCurHour = 0; tCurDay++;
            }
            if (tCurDay >= 30) { tCurDay = 0; tCurMonth++; }
            if (tCurMonth >= 12) { tCurMonth = 0; tCurYear++; }
        }
    }

    public static float TimeBoost
    {
        get { return tBoost; }
        set 
        { 
            tBoost = value; 
            if (tBoost >= 1)
            { tUnitTime = tBoost * 60; }
            else { tBoost = 1;  tUnitTime = 60; }
        }
    }

    public static bool IsPaused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }

    public static int TimeYear
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurYear); }
    }

    public static int TimeMonth
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurMonth); }
    }

    public static int TimeDay
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurDay); }
    }

    public static int TimeHour
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurHour); }
    }

    public static int TimeMinute
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurMinute); }
    }

    public static int TimeSecond
    {
        // Round UP.
        get { return Mathf.CeilToInt(tCurSecond); }
    }
}
