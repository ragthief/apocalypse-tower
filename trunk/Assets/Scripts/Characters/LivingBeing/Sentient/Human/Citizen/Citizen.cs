using UnityEngine;
using System;
using System.Collections.Generic;

public class Citizen : Human {

    private bool goingToLocation;
    private List<Vector2> pathToLocation = new List<Vector2>();
    private Vector2 nextInstruction;

    public override void Start() {
        base.Start();
    }

    public void FixedUpdate() {

        if (goingToLocation) {
            if (nextInstruction.x == (int) Math.Round(transform.position.x, MidpointRounding.AwayFromZero) && nextInstruction.y == (int) Math.Round(transform.position.y, MidpointRounding.AwayFromZero)) { // Update nextInstruction.
				pathToLocation.RemoveAt(0);
				if (pathToLocation.Count > 0) {
					nextInstruction = pathToLocation[0];
                } else {
                    goingToLocation = false;
                }
            }

            if (!isLadder) {
                if (nextInstruction.x > Math.Round(transform.position.x)) {
                    rigidbody2D.velocity = Vector2.right * moveSpeed;
                } else if (nextInstruction.x < Math.Round(transform.position.x)) {
                    rigidbody2D.velocity = -Vector2.right * moveSpeed;
                } else {
                    if (nextInstruction.y > (int)transform.position.y) {
                        UseLadder(up);
                    } else {
                        UseLadder(down);
                    }
                    Vector2 directionToLadder = new Vector2(World.GetRoom((int) Math.Round(transform.position.x, MidpointRounding.AwayFromZero), (int)transform.position.y).ladderObject.transform.position.x - transform.position.x, 0);
                    rigidbody2D.velocity = directionToLadder.normalized * moveSpeed; 
                }
            }
        }
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
            } else if (ascendFlag) {
                rigidbody2D.velocity = new Vector2(0, transform.localScale.y * moveSpeed);
                isAscending = true;
            }
        } else {
            //Set the character's velocity to moveSpeed in the x direction.
            if (!goingToLocation) {
                rigidbody2D.velocity = new Vector2(moveSpeed * direction, 0);
            }
        }
    }

    public override void OnGUI() {
        base.OnGUI();
        if (Input.GetMouseButtonDown(2)) {
            goingToLocation = false;
            Vector2 currentLocation = new Vector2((int) Math.Round(transform.position.x, MidpointRounding.AwayFromZero), (int) Math.Round(transform.position.y, MidpointRounding.AwayFromZero));
            Vector2 endLocation = new Vector2((int) Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, MidpointRounding.AwayFromZero), (int) Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, MidpointRounding.AwayFromZero));
            pathToLocation = World.FindPath(currentLocation, endLocation);
            if (pathToLocation != null && pathToLocation.Count > 0) {
                nextInstruction = pathToLocation[0];
                goingToLocation = true;
            }
        }
    }

    //Sets flags so that on the next collision with a ladder that's above the citizen, then will climb it in the direction specified.
    protected void UseLadder(int direction) {
        if (direction == 1) {
            FindLadder();
            ascendFlag = true;
            descendFlag = false;
        } else {
            FindLadder();
            descendFlag = true;
            ascendFlag = false;
        }
    }

    //Layer flag methods
    //Walk on any platform above the ground floor.
    protected void walkPlatform() {
        isGrounded = true;
        isFloored = true;
        this.gameObject.layer = platformLayer;
    }
    //Walk on ground
    protected void walkGround() {
        Debug.Log("Walk ground");
        isGrounded = true;
        FindLadder();
    }
    //Called when collision detected by ladder, and the citizen is flagged to ascend the ladder
    //Causes citizen to ignore various object to ascend ladder - layer designations in World.cs
    protected void ClimbUpLadder() {
        Debug.Log("Climb up ladder");
        this.gameObject.layer = ascendLayer;
    }
    //Called when collision detected by ladder, and the citizen is flagged to descend the ladder
    //Causes citizen to ignore various objects to descend ladder - layer designations in World.cs
    protected void ClimbDownLadder() {
        Debug.Log("Climb down ladder");
        this.gameObject.layer = descendLayer;
    }
    //Ladder called whenever you want to find a ladder
    protected void FindLadder() {
        Debug.Log("Find ladder");
        this.gameObject.layer = findLadderLayer;
    }
    //Ladder called when you arrive at bottom of transport object and want to move away from it before re-enabling collisions
    protected void ArriveAtBottom() {
        Debug.Log("Arrive Bottom");
        this.gameObject.layer = landingLayer;
    }

    protected void setLadderMoves(int floorNum) {
        //Moves citizen to a specified floor level

    }

    public override void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.gameObject.tag == "CheckLadder") {
            Debug.Log("Check Ladder!");
            if ((trigger.transform.position.y > this.transform.position.y) && useNextLadder && ascendFlag) {
                Debug.Log("Ladder is above");
                FindLadder();
            } else if ((trigger.transform.position.y < this.transform.position.y) && useNextLadder && descendFlag) {
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
            } else {
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
                ascendFlag = false;
                descendFlag = false;
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
}
