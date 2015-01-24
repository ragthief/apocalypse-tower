using UnityEngine;
using System.Collections;

public class gameGUI : MonoBehaviour {
    /* TODO:
     * - Intergrate Pause Function in void wndPause
     * - Get NPC Data from selected NPC in void wndNPCData
     * - Get Statistics in void wndStats
     */

    // This is for the Matrix, AKA the thing that makes our GUI Resolution independent.
    public float mWidth = 1280.0f;
    public float mHeight = 1024.0f;

    public Vector3 mScale;
    public Matrix4x4 mMat;

    public bool bBuildMenu;     // Display Build Menu?
    public bool bStatistics;    // Display Statistics?
    public bool bTimeTool;      // Display Time Controls?
    public bool bNPCData;       // Display NPC Information?
    public bool bShowPause;     // Display Pause Menu?

    // NOTE: For Start positions let the player drag it to the desired position.
    public Rect rBuildMenu = new Rect(0, 0, 200, 250);
    public Rect rStatistics = new Rect(0, 0, 800, 600);  // Someone who is still using a old monitor is going to kill me for this.
    public Rect rTimeTool = new Rect(0, 0, 100, 50);
    public Rect rNPCData = new Rect(0, 0, 200, 250);
    public Rect rShowPause = new Rect(540, 387, 200, 250);  // Do some magic here to make it fit properly.

    // This is for the three buttons and the box under them.
    public Rect rMBox = new Rect(455, 974, 360, 150);    // DO NOT TOUCH THIS, IT TOOK 5 COFFEES AND A MUFFIN TO FIGURE THIS OUT.
    public Rect rMBtn1 = new Rect(460, 924, 80, 80);
    public Rect rMBtn2 = new Rect(550, 924, 80, 80);
    public Rect rMBtn3 = new Rect(640, 924, 80, 80);
    public Rect rMBtn4 = new Rect(730, 924, 80, 80);

    public string iPlatform = "Windows";

    void OnGUI()
    {
        // Prepare our GUI Matrix
        mScale.x = Screen.width / mWidth;
        mScale.y = Screen.height / mHeight;
        mScale.z = 1;

        mMat = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, mScale);

        // Programmer pun: Oh if you could see me now. (The Script)
        if (bBuildMenu) { rBuildMenu = GUI.Window(0, rBuildMenu, wndBuild, "Building Tools"); }
        if (bStatistics) { rStatistics = GUI.Window(1, rStatistics, wndStats, "Statistics"); }
        if (bTimeTool) { rTimeTool = GUI.Window(2, rTimeTool, wndTime, "Time Manipulation"); }
        if (bNPCData) { rNPCData = GUI.Window(3, rNPCData, wndNPCData, "About this NPC"); }
        if (bShowPause) { rShowPause = GUI.Window(4, rShowPause, wndPause, "Game Paused"); }

        // Only display if the game is NOT paused.
        if (!bShowPause)
        {
            GUI.Box(rMBox, "");
            if (GUI.Button(rMBtn1, "Building")) { bBuildMenu = true; }
            if (GUI.Button(rMBtn2, "Statistics")) { bStatistics = true; };
            if (GUI.Button(rMBtn3, "The Time")) { bTimeTool = true; }
            if (GUI.Button(rMBtn4, "Pause")) { bShowPause = true; }
        }

        // Push our GUI Material to the GUI Matrix so it can be scaled down
        GUI.matrix = mMat;
    }

    void wndBuild(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 180, 20));
        if (GUI.Button(new Rect(180, 0, 20, 20), "X")) { bBuildMenu = false; }
        // CONTENT HERE
    }

    void wndStats(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 780, 20));
        if (GUI.Button(new Rect(780, 0, 20, 20), "X")) { bStatistics = false; }
        // CONTENT HERE
    }

    void wndTime(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 80, 20));
        if (GUI.Button(new Rect(80, 0, 20, 20), "X")) { bTimeTool = false; }
        // CONTENT HERE
    }

    void wndNPCData(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 230, 20));
        if (GUI.Button(new Rect(230, 0, 20, 20), "X")) { bNPCData = false; }
        // CONTENT HERE
    }

    void wndPause(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 200, 20));
        // We don't need to go all complicated on this for now, just use GUILayout.
        if (GUILayout.Button("Resume")) { bShowPause = false; }
        GUILayout.Button("Settings");
        GUILayout.Button("Quit to Menu");
        if (GUILayout.Button("Quit to " + iPlatform)) { Application.Quit(); } // In case we also do a build for Mac or Linux
    }
}
