using UnityEngine;
using System.Collections;

public class Sentient : LivingBeing {
    //Static variables
    public static int openerID = 0;
    public static int findID = 0;
    public static bool menuOpen = false;
    public static bool deleteOpenMenu = false;
    public static bool destroyOldIndicators = false;

    //Ints
    public int id;
    public int randNum;
    int numWallsHit = 0;
    int maxWallsHit;
    public int dexterity = 10;
    public int endurance = 10;
    public int intelligence = 10;
    public int charisma = 10;
    public int maxStats = 100;
    int numLaddaz = 0;
    
    //Ladder targets
    int targetFloor = 4;
    int currentFloor = 0;

    //Layer ints
    private int groundedLayer = 9;
    private int ascendLayer = 14;
    private int descendLayer = 15;
    private int platformLayer = 19;
    private int findLadderLayer = 21;
    private int landingLayer = 22;
    
    //Ladder Ints
    int ladderReactionDelay = -1;
    int up = 1;
    int down = 0;

    //Strings
    public string name;
    public string sex;

    //Bools
    private bool isMale = false;
    private bool oldIndicator = false;
    public bool displayNPCInformation;
    
    //Ladder bools
    public bool isLadder = false;
    public bool isFloored = false;
    public bool descendFlag = false;
    public bool ascendFlag = true;
    public bool useNextLadder = true;
    public bool isDescending = false;
    public bool isAscending = false;
    public bool skipTrigger = false;

    //Objects
    public GameObject citizenIndicator;
    public Rect NPCData = new Rect(0, 0, 200, 250);
    public Rect xButton = new Rect(230, 20, 20, 20);
    public Transform ladderPosition;

    // Use this for initialization
    public override void Start() {
        base.Start();
        assignStats(maxStats);
        World.initialiseLayers();
        //Currently the citizen bouces off of walls this number of times before going to a new floor.
        maxWallsHit = getRand(4);
    }

    // Update is called once per frame
    public override void Update() {
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
            else if (ascendFlag) {
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * moveSpeed);
                isAscending = true;
            }
            /*
            else {
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * -moveSpeed);
            }
            */
        }
        //Else move around normally
        else {
            //Set the character's velocity to moveSpeed in the x direction.
            rigidbody2D.velocity = new Vector2(moveSpeed * direction, 0);
        }
    }

    public virtual void OnMouseDown() {
        //If indicator already being displayed.
        if (oldIndicator) {
            Indicator.DestroyAll();
            oldIndicator = false;
        }
        else {
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

    public virtual void createIndicator() {
        citizenIndicator = Instantiate(Resources.Load("Prefabs/Indicator"), new Vector3(transform.position.x, transform.position.y + .7f, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
        citizenIndicator.transform.parent = this.transform;
        oldIndicator = true;
    }

    public virtual void OnGUI() {
        //If flag received to delete all open citizen menus, set displayNPCInformation flag to false so no window is drawn.
        if (deleteOpenMenu && displayNPCInformation) {
            displayNPCInformation = false;
            deleteOpenMenu = false;
            menuOpen = false;
        }
        //If flag specified, draw NPC information
        if (displayNPCInformation) {
            NPCData = GUI.Window(3, NPCData, wndNPCData, "About this NPC");
        }
    }

    //Information contained inside of NPC data window
    public virtual void wndNPCData(int windowid) {
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
                         "Floor: " + currentFloor + "\n" +
                         "Target Floor: " + targetFloor + "\n" +
                         "Ascend Flag: " + ascendFlag + "\n" +
                         "Descend Flag: " + descendFlag + "\n" +
                         "Use next ladder: " + useNextLadder);
    }

    public virtual void assignStats(int randCeiling) {
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
        randNum = Random.Range(0, randCeiling);
        dexterity += randNum;
        maxStats -= randNum;
        randNum = Random.Range(0, randCeiling);
        endurance += randNum;
        maxStats -= randNum;
        randNum = Random.Range(0, randCeiling);
        intelligence += randNum;
        randCeiling -= randNum;
        //Overall remainder assigned to charisma.
        charisma += randCeiling;
    }

    public virtual int getRand(int range) {
        int myRand = Random.Range(range, 0);
        return myRand;
    }

    public virtual string getName() {
        return name;
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
    }
    //Ladder called when you arrive at bottom of transport object and want to move away from it before re-enabling collisions
    void ArriveAtBottom() {
        Debug.Log("Arrive Bottom");
        this.gameObject.layer = landingLayer;
    }

    void setLadderMoves(int floorNum) {
        //Moves citizen to a specified floor level

    }

    public override void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.gameObject.tag == "CheckLadder") {
            Debug.Log("Check Ladder!");
            if ((trigger.transform.position.y > this.transform.position.y) && useNextLadder && ascendFlag) {
                Debug.Log("Ladder is above");
                FindLadder();
            }
            else if ((trigger.transform.position.y < this.transform.position.y) && useNextLadder && descendFlag) {
                Debug.Log("Ladder is below");
                FindLadder();
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D col) {
        //Handles case whenever citizen is landing I'm pretty sure - The originator
        if ((col.gameObject.tag == "Floor" || col.gameObject.tag == "Ground") && isLadder) {
            isGrounded = true;
            isFloored = true;
            if (isDescending == true) {
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
        if (col.gameObject.tag == "Ladder" && ladderReactionDelay < 0) {
            isLadder = true;
            ladderReactionDelay = -1;
            ladderPosition = col.transform;
            if (ascendFlag) {
                ClimbUpLadder();
                Debug.Log("Going up");
            }
            if (descendFlag) {
                ClimbDownLadder();
                Debug.Log("Going down");
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
                    //If the citizen is currently below the target floor, ascend
                    if (currentFloor < targetFloor) {
                        useNextLadder = true;
                        ascendFlag = true;
                        descendFlag = false;
                        Debug.Log("Mark ascend");
                    }
                    //Else if the citizen is above target floor, descend
                    else if (currentFloor > targetFloor) {
                        useNextLadder = true;
                        descendFlag = true;
                        ascendFlag = false;
                        Debug.Log("Mark descend");
                    }
                    //Else carry on
                    else {
                        numWallsHit = 0;
                        ascendFlag = false;
                        descendFlag = false;
                    }
                }
            }
        }
    }
    public override void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground" && isLadder) {
            //Change into transport citizen
            ClimbUpLadder();
        }
        if (col.gameObject.tag == "Ladder") {
            if (isAscending) {
                numLaddaz++;
                isLadder = false;
                //Change back to regular citizen
                walkPlatform();
                ladderReactionDelay = 1;
                isAscending = false;
                isDescending = false;
                currentFloor++;
            }
            if (isDescending) {
                currentFloor--;
            }
        }
        if (col.gameObject.tag == "Floor" && isLadder) {
            if (skipTrigger) {
                //FindLadder();
                skipTrigger = false;
            }
            if (isLadder) {
                isFloored = false;
            }
        }
    }

    //Option to set the target floor externally
    public virtual void setTargetFloor(int floorToTarget) {
        targetFloor = floorToTarget;
    }
}
