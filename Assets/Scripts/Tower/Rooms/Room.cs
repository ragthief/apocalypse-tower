using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	protected int height = 1;           //Using initialise in tower didn't do anything
	protected int width = 2;
    protected bool moved = false; //used for the gui when placing building
    protected bool isPlaced;    // Whether the room is still attached to the cursor, or weather the room has been constructed.
    protected bool canAccess = false;
    public Texture2D badPlacement;
	private List<Vector2> badLocations = new List<Vector2>();
    int id = 0;
    static int findID = 0;
    private bool firstClick = true;
    private bool createLadderFlag = false;
    private string transPath = "Prefabs/Transportation/";

    public static bool menuOpen = false;
    public static bool deleteOpenMenu = false;
    public static int openerID = 0;

    private bool roomLeft = true;
    private bool roomRight = true;

    public GameObject leftBuildingEdge = null;
    public GameObject rightBuildingEdge = null;

    private float buildingEdgeYOffset = 0.95099f;
    private float leftBuildingEdgeXOffset = -1.14f;
    private float rightBuildingEdgeXOffset = 1.14f;

    private static int statBoxLeft = 0;
    private static int statBoxTop = 0;
    private static int statBoxWidth = 200;
    private static int statBoxHeight = 250;

    public GameObject ladderObject = null;
    public GameObject descendLadder = null;
    public GameObject floorObject = null;
    public GameObject ceilingObject = null;
    public float floorOffset = -.5298f;
    public float ceilingOffset = .4700f;
    public float ladderYOffset = .055132f;
    public Rect RoomData = new Rect(statBoxLeft, statBoxTop, statBoxWidth, statBoxHeight);
    public bool displayRoomInformation;       // Display NPC Information?
    public Rect ladderButton = new Rect(statBoxLeft, 400, statBoxWidth, statBoxHeight);
    public bool hasLadder = false;

    public static string[] AvailableRooms()
    {
        return new string[]
		{
			"GeneralStore",
            "ReinforcedRustyShack"
		};
    }

    public virtual void Initialise()
    {
        height = 1;           //Overriden Method
	    width = 2;
    }

    // Use this for initialization
    void Start()
    {
        badPlacement = (Texture2D)Resources.Load("BadPlacement");
    }

    // Will be called when the user has clicked to confirm they want the room to be placed.
    protected void Construct()
    {
        isPlaced = true;
        this.id = findID;
        findID++;
		Tower.AddRoom(this);
        floorObject = Instantiate(Resources.Load(transPath + "Floor"), new Vector3(transform.position.x, transform.position.y + floorOffset, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        ceilingObject = Instantiate(Resources.Load(transPath + "Ceiling"), new Vector3(transform.position.x, transform.position.y + ceilingOffset, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        leftBuildingEdge = Instantiate(Resources.Load("Prefabs/LeftBuildingEdge"), new Vector3(transform.position.x + leftBuildingEdgeXOffset, transform.position.y + buildingEdgeYOffset, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        rightBuildingEdge = Instantiate(Resources.Load("Prefabs/RightBuildingEdge"), new Vector3(transform.position.x + rightBuildingEdgeXOffset, transform.position.y + buildingEdgeYOffset, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
    }

	public int Height{
		get { return height; }
	}

	public int Width{
		get { return width; }
	}

    public bool roomPlaced()
    {
        return isPlaced;
    }

	public virtual bool BuildOnTop(Room room){
		return true;
	}

    public bool isAccessible()
    {
        return canAccess;
    }

    // Called every frame.
    void Update()
    {
        if (!isPlaced)
        {
            moved = false;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Mathf.Round(transform.position.x) != Mathf.Round(pos.x) || Mathf.Round(transform.position.y) != Mathf.Round(pos.y))
            {
                moved = true;
                transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (World.Build(this, (int)transform.position.x, (int)transform.position.y))
                {
                    Construct();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //right click cancels placement
                Destroy(this.gameObject);
            }

            //escape cancels placement
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (World.CheckLeft(this, (int)transform.position.x, (int)transform.position.y))
            {
                roomLeft = true;
                Destroy(leftBuildingEdge);
            }
            else
            {
                roomLeft = false;
            }
            if (World.CheckRight(this, (int)transform.position.x, (int)transform.position.y))
            {
                roomRight = true;
                Destroy(rightBuildingEdge);
            }
            else
            {
                roomRight = false;
            }
        }
    }

    void OnGUI()
    {
        if (deleteOpenMenu && displayRoomInformation)
        {
            displayRoomInformation = false;
            deleteOpenMenu = false;
            menuOpen = false;
        }
        //If flag specified, draw NPC information
        if (displayRoomInformation)
        {
            RoomData = GUI.Window(3, RoomData, wndNPCData, "Room Information");
            if (GUI.Button(ladderButton, "createLadder")) {
                if (!createLadderFlag) {
                    createLadder();
                    createLadderFlag = true;
                }
                else if (createLadderFlag) {
                    destroyLadder();
                    createLadderFlag = false;
                }
            }
            //NPCData.x = this.transform.position.x;
            //NPCData.y = this.transform.position.y+50;
        }
        if (!isPlaced)
        {
            if (moved)
            {
                badLocations = new List<Vector2>();
                //this is used to display red squares over the building to show it can't be built
                int y = (int)transform.position.y;
                int x = (int)transform.position.x;
                for (int i = y; i > y - Height; i--)
                {
                    for (int k = x; k < x + Width; k++)
                    {
                        bool check = World.CheckBuildingBoundry(this, k, i, i == y - Height + 1);
                        if (!check)
                        {
                            badLocations.Add(new Vector2(k, i));
                        }
                    }
                }
            }

            //draw a red rectangle over the bad sections of the the building that don't meet building requirements (must be built on ground, ontop of other buildings, within the world scale, etc.
            foreach (Vector2 loc in badLocations)
            {
                int scale = 300 / World.orthoSize;

                //keep the red squares in line with the camera position
                float heightDiff = (Camera.main.transform.position.y - 2) * 2;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(loc.x - 1, (loc.y - heightDiff - 3.5f) * -1, 0));

                GUI.BeginGroup(new Rect(screenPos.x, screenPos.y, scale, scale));
                GUI.DrawTexture(new Rect(0, 0, scale, scale), badPlacement);
                GUI.EndGroup();
            }
        }
    }

    void wndNPCData(int windowid)
    {
        //Overriden with pertinent information
        string displayFrame = this.displayInformation();

        GUI.DragWindow(new Rect(0, 0, 230, 20));
        GUI.Box(RoomData, "\n" + displayFrame);
    }

    public virtual string displayInformation()
    {
        string returnString = "Returning Default";
        return returnString;
    }
    
    void OnMouseDown()
    {
        Debug.Log("You clicked a room!");
        if (!menuOpen && !firstClick)
        {
            displayRoomInformation = true;
            openerID = this.id;
            menuOpen = true;
        }
        else if (firstClick)
        {
            firstClick = false;
        }
        //If the citizen clicked is the same as the menu currently open, close the menu.
        else if (this.id == openerID)
        {
            displayRoomInformation = false;
            menuOpen = false;
        }
        else
        {
            deleteOpenMenu = true;
            menuOpen = false;
        }
    }

    public bool getRoomLeft()
    {
        return roomLeft;
    }

    public bool getRoomRight()
    {
        return roomRight;
    }

    void createLadder() {
        float xOffset = transform.position.x - .5f;
        ladderObject = Instantiate(Resources.Load(transPath + "Ladder"), new Vector3(xOffset, transform.position.y + ladderYOffset, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        hasLadder = true;
        World.ConstructGraph();
    }

    void destroyLadder() {
        Destroy(ladderObject);
        hasLadder = false;
        World.ConstructGraph();
    }

    void OnDestroy()
    {
        findID--;
    }
}