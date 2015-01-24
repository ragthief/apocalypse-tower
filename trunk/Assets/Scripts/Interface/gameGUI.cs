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

    public bool bIsBuilding;

    // Grid size for Building
    public float fGridSize = 1.0f;

    // NOTE: For Start positions let the player drag it to the desired position.
    public Rect rBuildMenu = new Rect(0, 0, 200, 800);
    public Rect rStatistics = new Rect(0, 0, 800, 600);  // Someone who is still using a old monitor is going to kill me for this.
    public Rect rTimeTool = new Rect(0, 0, 275, 65);
    public Rect rNPCData = new Rect(0, 0, 200, 250);
    public Rect rShowPause = new Rect(540, 387, 200, 250);  // Do some magic here to make it fit properly.

    // This is for the three buttons and the box under them.
    public Rect rMBox = new Rect(455, 974, 360, 150);    // DO NOT TOUCH THIS, IT TOOK 5 COFFEES AND A MUFFIN TO FIGURE THIS OUT.
    public Rect rMBtn1 = new Rect(460, 924, 80, 80);
    public Rect rMBtn2 = new Rect(550, 924, 80, 80);
    public Rect rMBtn3 = new Rect(640, 924, 80, 80);
    public Rect rMBtn4 = new Rect(730, 924, 80, 80);

    //Population Stats

    // The Clock
    public Rect rClock = new Rect(10, 10, 50, 40);
    public int tBoost;

    public string iPlatform = "Windows";

	public BuildingWigit buildingWigit;

    void Start()
    {
        tBoost = 1;
        //Tower.AddRoom(new TestRoom());
        envTimeOfDay.TimeBoost = tBoost;
        DetectOS();

		World.NewWorld(25, 25);
    }

    void Update()
    {
        //Tower.UpdateRooms();
        envTimeOfDay.UpdateTime();
        if (bIsBuilding)
        {
            // You are building now, enjoy.
        }

		//zooming the camera
		if (Input.GetKeyDown(KeyCode.LeftBracket)) {
			World.ZoomIn();
		}

		if (Input.GetKeyDown(KeyCode.RightBracket)) {
			World.ZoomOut();
		}

		//moving the camera
		if (Input.GetKey(KeyCode.W)) {//|| Input.GetKey(KeyCode.UpArrow)) {
			//move camera up
			Camera.main.transform.position += new Vector3(0, .2f, 0);

			World.CheckCameraYMax();
		}
		if (Input.GetKey(KeyCode.S)) { //|| Input.GetKey(KeyCode.DownArrow)) {
			//move camera down
			Camera.main.transform.position -= new Vector3(0, .2f, 0);

			World.CheckCameraYMin();
		}
		if (Input.GetKey(KeyCode.A)) { //|| Input.GetKey(KeyCode.LeftArrow)) {
			//move camera Left
			Camera.main.transform.position -= new Vector3(.2f, 0, 0);

			World.CheckCameraXMin();
		}
		if (Input.GetKey(KeyCode.D)) {   // || Input.GetKey(KeyCode.RightArrow)) {
			//move camera right
			Camera.main.transform.position += new Vector3(.2f, 0, 0);

			World.CheckCameraXMax();
		}
    }

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
        if (!bShowPause || !bIsBuilding)
        {
            GUI.Box(rMBox, "");
			if (GUI.Button(rMBtn1, "Building")) { buildingWigit.Toggle(BuildingWigit.TYPE.ROOM);}//bBuildMenu = true; }
            if (GUI.Button(rMBtn2, "Statistics")) { bStatistics = true; };
            if (GUI.Button(rMBtn3, "The Time")) { bTimeTool = true; }
            if (GUI.Button(rMBtn4, "Pause")) { 
                bShowPause = true;
                bBuildMenu = false;
                bStatistics = false;
                bTimeTool = false;
                envTimeOfDay.TimeBoost = 0;
            }
            // Draw the clock
            int tHour = envTimeOfDay.TimeHour;
            int tMinute = envTimeOfDay.TimeMinute;

            int tDay = envTimeOfDay.TimeDay;
            int tMonth = envTimeOfDay.TimeMonth;
            int tYear = envTimeOfDay.TimeYear;
            GUI.Box(rClock, tHour + ":" + tMinute + "\n" + tDay + "/" + tMonth + "/" + tYear);
        }

        // Push our GUI Material to the GUI Matrix so it can be scaled down
        GUI.matrix = mMat;

		//upper-right corner menu
		GUI.BeginGroup(new Rect(Screen.width - 200, 0, 200, 200));

		//zoom buttons
		if (GUI.Button(new Rect(0,10,20,20), "+")) { World.ZoomIn(); };
		if (GUI.Button(new Rect(5,35,20,20), "-")) { World.ZoomOut(); };

        //Population Stats
        GUI.Label(new Rect(40, 10, 60, 20), Tower.getCurrentPopulation() + "/" + Tower.getPopulationCap());

		GUI.EndGroup();

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
        if (GUI.Button(new Rect(780, 0, 20, 20), "X")) {
            bStatistics = false; }
            // CONTENT HERE
        }

    void wndTime(int windowid)
    {
        GUI.DragWindow(new Rect(0, 0, 275-20, 20));
        if (GUI.Button(new Rect(275-20, 0, 20, 20), "X")) { bTimeTool = false; }
        if (GUI.Button(new Rect(5, 20, 40, 40), "1X")) { envTimeOfDay.TimeBoost = 1; }
        if (GUI.Button(new Rect(50, 20, 40, 40), "4X")) { envTimeOfDay.TimeBoost = 4; }
        if (GUI.Button(new Rect(95, 20, 40, 40), "12X")) { envTimeOfDay.TimeBoost = 12; }
        if (GUI.Button(new Rect(140, 20, 40, 40), "32X")) { envTimeOfDay.TimeBoost = 32; }
        if (GUI.Button(new Rect(185, 20, 40, 40), "64X")) { envTimeOfDay.TimeBoost = 64; }
        if (GUI.Button(new Rect(230, 20, 40, 40), "PAU")) { envTimeOfDay.TimeBoost = 0; }
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
        if (GUILayout.Button("Resume")) { bShowPause = false; envTimeOfDay.TimeBoost = 1; }
        GUILayout.Button("Settings");
        GUILayout.Button("Quit to Menu");
        if (GUILayout.Button("Quit to " + iPlatform)) { Application.Quit(); } // In case we also do a build for Mac or Linux
    }

    void DetectOS()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            iPlatform = "Editor";
        } else
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            iPlatform = "Editor";
        } else
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            iPlatform = "Windows";
        } else
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            iPlatform = "OS X";
        } else
        if (Application.platform == RuntimePlatform.LinuxPlayer)
        {
            iPlatform = "Linux";
        } else
        { iPlatform = "Unknown"; }
    }
}
