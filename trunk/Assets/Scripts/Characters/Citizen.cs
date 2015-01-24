using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Citizen : MonoBehaviour
{
    public float moveSpeed = 2f;		//Speed of the citizen
    int id;
    string name;
    string sex;
    int dexterity = 10;
    int endurance = 10;
    int intelligence = 10;
    int charisma = 10;
    int maxStats = 100;
    int right = 1;
    int left = -1;
    int randNum;
    int numWallsHit = 0;
    bool isGrounded;
    static int findID = 0;
    int up = 1;
    int down = 0;
    int numLaddaz = 0;

    int maxWallsHit;

    private bool isMale = false;

    //Layer ints
    private int groundedLayer = 9;
    private int ascendLayer = 14;
    private int descendLayer = 15;
    private int platformLayer = 19;
    private int findLadderLayer = 21;
    private int landingLayer = 22;

    public static bool menuOpen = false;
    public static bool deleteOpenMenu = false;
    public static int openerID = 0;
    private bool isLadder = false;
    private bool isFloored = false;
    private bool descendFlag = false;
    private bool ascendFlag = true;
    private bool useNextLadder = true;
    int ladderReactionDelay = -1;
    private bool isDescending = false;
    private bool isAscending = false;
    private bool skipTrigger = false;
    private static List<int> floorMoves = new List<int>();
    private int floorLevel = 0;
    public static bool displayIndicator = false;
    public static bool destroyOldIndicators = false;
    private bool oldIndicator = false;

    public GameObject citizenIndicator;

    public Rect NPCData = new Rect(0, 0, 200, 250);
    public Rect xButton = new Rect(230, 20, 20, 20);
    public bool displayNPCInformation;       // Display NPC Information?
    private Transform ladderPosition;

    private SpriteRenderer renderer;			//Reference to the sprite renderer.
    private Transform frontCheck;		//Reference to object that checks for collisions in front of citizen

    private bool movingToLocation; //Determines if the character is following a path given to it.
    private Vector2 destination; //The location the character is moving to in its path.
    private List<Vector2> path; //The path the character will need to follow.
    private Vector2 nextNode; //Determines the next node we are going to in the path sequence.

    void Awake() {
        //Setting up the references.
        //renderer = transform.Find("body").GetComponent<SpriteRenderer>();
        //frontCheck = transform.Find("frontCheck").transform;
    }

    void Start() {
        GenerateName.initNames();
        //Assign ID number and increment for next character.
        id = findID;
        findID++;

        //Decide randomly if citizen is male or female
        randNum = Random.Range(0, 2);
        if (randNum > 0) {

            isMale = true;
            sex = "Male";
            name = GenerateName.getName('m');
        }
        else {
            isMale = false;
            sex = "Female";
            name = GenerateName.getName('f');
        }
        
        //Possible better way is to assign each stat a random number 1 - 4 to randomly determine the order they will be randomly allocated.
        //Allocate character stats having a max stat threshold represented by 'maxStats', and decrementing the random range.
        randNum = Random.Range(0, maxStats);
        dexterity += randNum;
        maxStats -= randNum;
        randNum = Random.Range(0, maxStats);
        endurance += randNum;
        maxStats -= randNum;
        randNum = Random.Range(0, maxStats);
        intelligence += randNum;
        maxStats -= randNum;
        //Overall remainder assigned to charisma.
        charisma += maxStats;

        World.initialiseLayers();

        Tower.addCitizen(this);

        maxWallsHit = getRand(4);
    }

    /*void Update() {
        //Use ladder reaction delay to avoid getting caught by ladder after landing
        if (ladderReactionDelay > 0) {
            ladderReactionDelay++;
            if (ladderReactionDelay > 10) {
                ladderReactionDelay = -1;
            }
        }
        //Ladder events triggered by 'isLadder' flag
        if (isLadder) {
            //If descending event flagged when ladder encountered
            if (descendFlag) {
                Debug.Log("I am actually descending");
                //If halfway down ladder, switch the layer to prepare for landing
                if (this.transform.position.y < ladderPosition.transform.position.y && isDescending) {
                    ArriveAtBottom();
                }
                //Move down the ladder
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * -moveSpeed);
                //Used to filter when landingLayer is triggered, in case ladders are doubled up
                isDescending = true;
            }
            //Else if ascend flag
            else if(ascendFlag){
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * moveSpeed);
                isAscending = true;
            }

            else
            {
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * -moveSpeed);
            }
        }
    }*/

    void FixedUpdate() {
        if (movingToLocation) {
            if (nextNode == new Vector2((int)transform.position.x, (int)transform.position.y)) {
                path.RemoveAt(0);
                if (path.Count > 0) {
                    nextNode = path[0];
                } else {
                    movingToLocation = false;
                }
            } else {
                if (nextNode.x > (int)transform.position.x) {
                    rigidbody2D.velocity = (Vector2.right) * moveSpeed;
                } else {
                    rigidbody2D.velocity = (-Vector2.right) * moveSpeed;
                }
            }
        }
    }

    void OnGUI() {
        //If flag received to delete all open citizen menus, set displayNPCInformation flag to false so no window is drawn.
        if (deleteOpenMenu && displayNPCInformation) {
            displayNPCInformation = false;
            deleteOpenMenu = false;
            menuOpen = false;
        }
        //If flag specified, draw NPC information
        if (displayNPCInformation) { 
            NPCData = GUI.Window(3, NPCData, wndNPCData, "About this NPC");
            //NPCData.x = this.transform.position.x;
            //NPCData.y = this.transform.position.y+50;
        }

        if (Input.GetMouseButtonDown(2)) {
            Vector3 screenToWorldLocation;
            screenToWorldLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            destination = new Vector2((int)screenToWorldLocation.x, (int)screenToWorldLocation.y);
            path = World.FindPath(new Vector2((int)transform.position.x, (int)transform.position.y) , destination);
            if (path != null) {
                movingToLocation = true;
                nextNode = path[0];
            }
        }
    }

    //Information contained inside of NPC data window
    void wndNPCData(int windowid) {
        GUI.DragWindow(new Rect(0, 0, 230, 20));
        if (GUI.Button(xButton, "X")) {
            displayNPCInformation = false; 
        }
        GUI.Box(NPCData, "\n" + "ID: " + id + "\n" + 
                         "Name: " + name + "\n" + 
                         "Sex: " + sex + "\n" +
                         "Dexterity: " + dexterity + "\n" + 
                         "Endurance: " + endurance + "\n" + 
                         "Intelligence: " + intelligence + "\n" + 
                         "Charisma: " + charisma + "\n" +
                         "Layer: " + this.gameObject.layer.ToString() + "\n" +
                         "Ladder Delay: " + this.ladderReactionDelay.ToString() + "\n" + numLaddaz + "\nisLadder: " + isLadder.ToString() + "\n" + "descendFlag: " + descendFlag.ToString() + "\n" + "ascendFlag: " + ascendFlag.ToString() +
                         "\n" + "isAscending: " + isAscending + "\n" + "isDescending: " + isDescending);
    }

    void OnCollisionEnter2D(Collision2D col) {
        //Handles case whenever citizen is landing I'm pretty sure - The originator
        if ((col.gameObject.tag == "Floor" || col.gameObject.tag == "Ground") && isLadder) {
            Debug.Log("I'm being called, mothafucka");
            isGrounded = true;
            isFloored = true;
            if (isDescending == true)
            {
                Debug.Log("Nigggaaaa");
                isLadder = false;
                isDescending = false;
            }

            if (col.gameObject.tag == "Floor") {
                walkGround();
            }
            else {
                walkGround();
            }
            ladderReactionDelay = 1;
            Flip();
        }
        //Basic Ladder encounter
        if (col.gameObject.tag == "Ladder" && ladderReactionDelay < 0 && useNextLadder) {
            isLadder = true;
            ladderReactionDelay = -1;
            ladderPosition = col.transform;
            if (this.transform.position.y < col.transform.position.y) {
                if(ascendFlag) {
                    ClimbUpLadder();
                }
                else {
                    SkipLadder();
                    skipTrigger = true;
                    //descendFlag = false;
                }
            }
            else if (this.transform.position.y > col.transform.position.y) {
                if(descendFlag) {
                    ClimbDownLadder();
                }
                else {
                    SkipLadder();
                    skipTrigger = true;
                    //ascendFlag = false;
                }
            }
        }

        if (col.gameObject.tag == "Ground") {
            walkGround();
        }
        if (col.gameObject.tag == "EdgeOfMap") {
            Flip();
        }
        if (col.gameObject.tag == "LeftBuildingEdge" || col.gameObject.tag == "RightBuildingEdge") {
            if (isFloored) {
                numWallsHit++;
                Flip();
                //Each time a wall is hit, check count to see if you should ascend or descent.
                if (numWallsHit > maxWallsHit) {
                    //If two walls hit, change layer to encounter descent transport objects.
                    RandomLadder();
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground" && isLadder) {
            //Change into transport citizen
            ClimbUpLadder();
        }
        if (col.gameObject.tag == "Ladder") 
        {   
            if(isAscending) {
                Debug.Log("You cunt");
                numLaddaz++;
                isLadder = false;
                //Change back to regular citizen
                walkPlatform();
                ladderReactionDelay = 1;
                isAscending = false;
                isDescending = false;
            }
        }
        if (col.gameObject.tag == "Floor" && isLadder) {
            if (skipTrigger)
            {
                //FindLadder();
                skipTrigger = false;
            }
            if (isLadder)
            {
                Debug.Log("Bitttttch");
                isFloored = false;
            }
        }
    }

    //Turn the citizen around
    public void Flip() {
        //Multiply the x component of localScale by -1.
        Vector3 citizenScale = transform.localScale;
        citizenScale.x *= -1;
        transform.localScale = citizenScale;
    }

    void SkipLadder() {
        isLadder = false;
        numWallsHit = 0;
        walkPlatform();
    }

    void RandomLadder() {
        int direction = getRand(100);
        numWallsHit = 0;
        if (direction > 50) {
            UseLadder(up);
        }
        else {
            UseLadder(down);
        }
    }

    //Sets flags so that on the next collision with a ladder that's above the citizen, then will climb it in the direction specified.
    void UseLadder(int direction) {
        if (direction == 1) {
            FindLadder();
            ascendFlag = true;
            descendFlag = false;
        }
        else {
            FindLadder();
            descendFlag = true;
            ascendFlag = false;
        }
    }

    //Layer flag methods
    //Walk on any platform above the ground floor.
    void walkPlatform() {
        isGrounded = true;
        isFloored = true;
        this.gameObject.layer = platformLayer;
    }
    //Walk on ground
    void walkGround() {
        Debug.Log("Walk ground");
        isGrounded = true;
        this.gameObject.layer = groundedLayer;
        UseLadder(up);
    }
    //Called when collision detected by ladder, and the citizen is flagged to ascend the ladder
    //Causes citizen to ignore various object to ascend ladder - layer designations in World.cs
    void ClimbUpLadder() {
        Debug.Log("Climb up ladder");
        this.gameObject.layer = ascendLayer;
    }
    //Called when collision detected by ladder, and the citizen is flagged to descend the ladder
    //Causes citizen to ignore various objects to descend ladder - layer designations in World.cs
    void ClimbDownLadder() {
        Debug.Log("Climb down ladder");
        this.gameObject.layer = descendLayer;
    }
    //Ladder called whenever you want to find a ladder
    void FindLadder() {
        Debug.Log("Find ladder");
        this.gameObject.layer = findLadderLayer;
        useNextLadder = true;
    }
    //Ladder called when you arrive at bottom of transport object and want to move away from it before re-enabling collisions
    void ArriveAtBottom() {
        Debug.Log("Arrive Bottom");
        this.gameObject.layer = landingLayer;
    }

    void setLadderMoves(int floorNum) {
        //Moves citizen to a specified floor level

    }

    void OnMouseDown() {
        //If indicator already being displayed.
        if (oldIndicator)
        {
            Indicator.DestroyAll();
            oldIndicator = false;
        }
        else
        {
            createIndicator();
        }
        
        //If no menu is open, and the button is clicked, draw the menu without worrying about anything.
        if (!menuOpen) {
            displayNPCInformation = true;
            openerID = this.id;
            menuOpen = true;
        }
        //If the citizen clicked is the same as the menu currently open, close the menu.
        else if (this.id == openerID) {
            displayNPCInformation = false;
            menuOpen = false;
        }
        else {
            deleteOpenMenu = true;
            menuOpen = false;
        }
    }

    private int getRand(int range)
    {
        int myRand = Random.Range(range, 0);
        return myRand;
    }

    void createIndicator()
    {
        citizenIndicator = Instantiate(Resources.Load("Prefabs/Indicator"), new Vector3(transform.position.x, transform.position.y + .7f, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        citizenIndicator.transform.parent = this.transform;
        oldIndicator = true;
    }

    public string getName() {
        return name;
    }
}
