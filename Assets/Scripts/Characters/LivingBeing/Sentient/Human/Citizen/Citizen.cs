using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Citizen : Human {

	private Vector2 velocityOnPath;
    private List<Vector2> pathToLocation = new List<Vector2>();
    private Vector2 nextInstruction;
	private bool newInstruction;
    private bool goingToLocation;
    private float verticalMoveSpeed;
    private bool useNextLadder;
    private bool onLadder;
    private bool onFloor;
    private float roamDirection;

    public override void Start() {
        verticalMoveSpeed = 1;
        roamDirection = 1;
        //base.Start();
    }

    public override void Update() {
        //base.Update();
    }

    public void FixedUpdate() {

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
	                if (!useNextLadder && nextInstruction.x > (int) transform.position.x) {
                        velocityOnPath = Vector2.right * moveSpeed;
                    } else if (!useNextLadder && nextInstruction.x < (int) transform.position.x) {
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
            velocityOnPath = new Vector2(World.GetRoom((int) transform.position.x, (int) Math.Round(transform.position.y, MidpointRounding.AwayFromZero)).verticalTransportObject.transform.position.x - transform.position.x, 0).normalized * moveSpeed;
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

    public override void OnGUI() {
        base.OnGUI();
        if (Input.GetMouseButtonDown(2)) {
            goingToLocation = false;
            Vector2 currentLocation = new Vector2((int) transform.position.x, (int) Math.Round(transform.position.y, MidpointRounding.AwayFromZero));
            Vector2 endLocation = new Vector2((int) Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (int) Math.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, MidpointRounding.AwayFromZero));
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
            Vector2 targetPosition = new Vector2((int) g.transform.position.x, (int)Math.Round(g.transform.position.y, MidpointRounding.AwayFromZero));
            Vector2 currentPosition = new Vector2((int) transform.position.x, (int)Math.Round(transform.position.y, MidpointRounding.AwayFromZero));
            pathToLocation = World.FindPath(currentPosition, targetPosition);
            if (pathToLocation != null && pathToLocation.Count > 0) {
				StartPath();
                break;
            }
        }
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
		gameObject.layer = transportLayer;
    }

	private void StopVerticalTransport() {
		isAscending = false;
		isDescending = false;
		ascendFlag = false;
		descendFlag = false;
		onLadder = false;
        useNextLadder = false;
		gameObject.layer = citizenLayer;
	}

    public override void OnTriggerEnter2D(Collider2D trigger) {
        if (isDescending && trigger.tag == "BottomTransportCheck") {
            StopVerticalTransport();
        }
    }

    public override void OnCollisionEnter2D(Collision2D col) {
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
                gameObject.layer = transportLayer;
            } else {
				onFloor = true;
				gameObject.layer = citizenLayer;
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
