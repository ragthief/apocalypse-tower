using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
    public int numWallsHit = 0;
    public int maxWallsHit;
    public int dexterity = 10;
    public int endurance = 10;
    public int intelligence = 10;
    public int charisma = 10;
    public int maxStats = 100;
    
    public const int up = 1;
    public const int down = 0;

    //Strings
    public string name;
    public string sex;

    //Bools
    private bool isMale = false;
    private bool oldIndicator = false;
    public bool displayNPCInformation;

    private Vector2 velocityOnPath;
    private List<Vector2> pathToLocation = new List<Vector2>();
    private Vector2 nextInstruction;
    private bool newInstruction;
    private bool goingToLocation;
    private float verticalMoveSpeed;
    private bool ascendFlag;
    private bool descendFlag;
    private bool isAscending;
    private bool isDescending;
    private bool useNextLadder;
    private bool onLadder;
    private bool onFloor;
    private float roamDirection;

    //Objects
    public GameObject citizenIndicator;
    public Rect NPCData = new Rect(0, 0, 200, 250);
    public Rect xButton = new Rect(230, 20, 20, 20);
    public Transform ladderPosition;

    // Use this for initialization
    public override void Start() {
        base.Start();
        assignStats(4);
        verticalMoveSpeed = 1;
        roamDirection = 1;
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        if (goingToLocation) {
            // Decide if the next instruction needs to be read from the path given to the human.
            if (!useNextLadder && nextInstruction.x == (int)transform.position.x && nextInstruction.y == (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) { // Update nextInstruction.
                pathToLocation.RemoveAt(0);
                if (pathToLocation.Count > 1) {
                    if (pathToLocation[1].y > (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) {
                        UseLadder(up);
                        pathToLocation.RemoveAt(0);
                    } else if (pathToLocation[1].y < (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) {
                        UseLadder(down);
                        pathToLocation.RemoveAt(0);
                    }
                    nextInstruction = pathToLocation[0];
                    newInstruction = true;
                } else if (pathToLocation.Count > 0) {
                    nextInstruction = pathToLocation[0];
                    newInstruction = true;
                } else {
                    goingToLocation = false;
                }
            }

            //Horizontal movement whilst on a path.
            if (!onLadder) {
                if (newInstruction) {
                    if (!useNextLadder && nextInstruction.x > (int)transform.position.x) {
                        velocityOnPath = Vector2.right * moveSpeed;
                    } else if (!useNextLadder && nextInstruction.x < (int)transform.position.x) {
                        velocityOnPath = -Vector2.right * moveSpeed;
                    }
                    newInstruction = false;
                }
                rigidbody2D.velocity = velocityOnPath;
            }
            //Horizontal movement whilst roaming.
        } else if (onFloor) {
            rigidbody2D.velocity = Vector2.right * roamDirection;
        }

        //Vertical movement.
        if (onLadder) {
            if (ascendFlag) {
                rigidbody2D.velocity = Vector2.up * verticalMoveSpeed;
                isAscending = true;
            } else if (descendFlag) {
                rigidbody2D.velocity = -Vector2.up * verticalMoveSpeed;
                isDescending = true;
            }
        }
    }

    public void StartPath() {
        newInstruction = false;
        if (pathToLocation[0].y > Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) {
            UseLadder(up);
            pathToLocation.RemoveAt(0);
            velocityOnPath = new Vector2(World.GetRoom((int)transform.position.x, (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero)).verticalTransportObject.transform.position.x - transform.position.x, 0).normalized * moveSpeed;
        } else if (pathToLocation[0].y < Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) {
            UseLadder(down);
            pathToLocation.RemoveAt(0);
            velocityOnPath = new Vector2(World.GetRoom((int)transform.position.x, (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero) - 1).verticalTransportObject.transform.position.x - transform.position.x, 0).normalized * moveSpeed;
        } else {
            newInstruction = true;
        }
        nextInstruction = pathToLocation[0];
        goingToLocation = true;
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

        if (Input.GetMouseButtonDown(2)) {
            goingToLocation = false;
            Vector2 currentLocation = new Vector2((int)transform.position.x, (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero));
            Vector2 endLocation = new Vector2((int)Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (int)Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, MidpointRounding.AwayFromZero));
            pathToLocation = World.FindPath(currentLocation, endLocation);
            if (pathToLocation != null && pathToLocation.Count > 0) {
                StartPath();
            }
        }
    }

    public void GoToNearest(string targetTag) {
        List<GameObject> targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
        var targetOrder = targets.OrderBy(o => (o.transform.position - transform.position).sqrMagnitude);
        goingToLocation = false;
        foreach (GameObject g in targetOrder) {
            Vector2 targetPosition = new Vector2((int)g.transform.position.x, (int)Math.Round(g.transform.position.y, MidpointRounding.AwayFromZero));
            Vector2 currentPosition = new Vector2((int)transform.position.x, (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero));
            pathToLocation = World.FindPath(currentPosition, targetPosition);
            if (pathToLocation != null && pathToLocation.Count > 0) {
                StartPath();
                break;
            }
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
        randNum = UnityEngine.Random.Range(0, 2);
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
        randNum = UnityEngine.Random.Range(0, randCeiling);
        dexterity += randNum;
        maxStats -= randNum;
        randNum = UnityEngine.Random.Range(0, randCeiling);
        endurance += randNum;
        maxStats -= randNum;
        randNum = UnityEngine.Random.Range(0, randCeiling);
        intelligence += randNum;
        randCeiling -= randNum;
        //Overall remainder assigned to charisma.
        charisma += randCeiling;
    }

    public virtual string getName() {
        return name;
    }

    protected void UseLadder(int direction) {
        useNextLadder = true;
        if (direction == 1) {
            ascendFlag = true;
            descendFlag = false;
        } else {
            descendFlag = true;
            ascendFlag = false;
        }
        gameObject.layer = Tower.TransportLayer;
    }

    private void StopVerticalTransport() {
        isAscending = false;
        isDescending = false;
        ascendFlag = false;
        descendFlag = false;
        onLadder = false;
        useNextLadder = false;
        gameObject.layer = Tower.CitizenLayer;
        if (goingToLocation && pathToLocation.Count > 0) {
            newInstruction = true;
        }
    }

    public override void OnTriggerEnter2D(Collider2D trigger) {
        if (isDescending && trigger.tag == "BottomTransportCheck") {
            StopVerticalTransport();
        }
    }

    public override void OnCollisionEnter2D(Collision2D col) {
        base.OnCollisionEnter2D(col);
        if (col.gameObject.tag == "Human") {
            Physics2D.IgnoreCollision(collider2D, col.collider);
        }

        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Floor" || col.gameObject.tag == "Ceiling") {
            onFloor = true;
        }

        if (col.gameObject.tag == "Ladder") {
            onLadder = true;
            if (useNextLadder) {
                onFloor = false;
                gameObject.layer = Tower.TransportLayer;
            } else {
                onFloor = true;
                gameObject.layer = Tower.CitizenLayer;
            }
        }

        if (col.gameObject.tag == "EdgeOfMap" || col.gameObject.tag == "LeftBuildingEdge" || col.gameObject.tag == "RightBuildingEdge") {
            roamDirection *= -1;
        }
    }

    public override void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ladder") {
            StopVerticalTransport();
        }
    }
}
