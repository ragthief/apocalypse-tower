using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World {

	private static Node[][] plots;
	public static int WIDTH = 0;
	public static int HEIGHT = 0;
	public static int orthoSize = 5;

    private static Graph pathfindingGraph;

    //Layer ints
    private static int groundedLayer = 9;
    private static int ascendLayer = 14;
    private static int descendLayer = 15;
    private static int platformLayer = 19;
    private static int findLadderLayer = 21;
    private static int landingLayer = 22;
    private static int ladderLayer = 12;


	public static void NewWorld(int width, int height){

		WIDTH = width;
		HEIGHT = height;

        pathfindingGraph = new Graph();

		plots = new Node[height][];

		for(int i = 0; i < height; i++){
			plots[i] = new Node[width];

			for(int k = 0; k < plots[i].Length; k++){
				plots[i][k] = new Node();

				if(i == 0){
					//the first row is the ground of the world
					plots[i][k].ground = true;
				}
			}
		}
	}

    public static Room GetRoom(int x, int y) {
        return plots[y][x].GetRoom();
    }

	public static bool Build(Room room, int x, int y){
		bool check = CheckBuildingBoundries(room, x, y);

		//everything checks out, update the plots
		if(check){

			for(int i = y; i > y - room.Height; i--){
				for(int k = x; k < x + room.Width; k++){
					plots[i][k].BuildRoom(room);
				}
			}
		}

		return check;

	}

    public static bool CheckRight(Room room, int nextX, int nextY) {
        int x = nextX + 2;
        int y = nextY;
        for (int i = y; i > y - room.Height; i--)
        {
            for (int k = x; k < x + room.Width; k++)
            {
                if (plots[i][k].open)
                {
                    //plot isn't open, can't build there
                    return false;
                }
            }
        }
        return true;
    }

    public static bool CheckLeft(Room room, int nextX, int nextY) {
        int x = nextX - 2;
        int y = nextY;
        for (int i = y; i > y - room.Height; i--)
        {
            for (int k = x; k < x + room.Width; k++)
            {
                if (plots[i][k].open)
                {
                    //plot isn't open, can't build there
                    return false;
                }
            }
        }
        return true;
    }

	public static bool CheckBuildingBoundries(Room room, int x, int y)
	{
		//check if room is being built outside of the bounds of the world
		
		if(x < 0 || y - room.Height + 1 < 0){
			return false;
		}
		
		if(x + room.Width - 1 >= WIDTH || y >= HEIGHT){
			return false;
		}
		
		for(int i = y; i > y - room.Height; i--){
			for(int k = x; k < x + room.Width; k++){
				if(!plots[i][k].open){
					//plot isn't open, can't build there
					return false;
				}
			}
		}
		
		//the room can not be built overtop nothing
		for(int k = x; k < x + room.Width; k++){
			if(!plots[y - room.Height + 1][k].ground){
				if(!plots[y - room.Height][k].BuildOnTop(room)){
					return false;
				}
			}
		}

		return true;
	}

	public static bool CheckBuildingBoundry(Room room, int x, int y, bool checkBelow)
	{
		//check if room is being built outside of the bounds of the world

		//Debug.Log(x + ", " + y);
		
		if(x < 0 || y < 0){
			return false;
		}
		
		if(x >= WIDTH || y >= HEIGHT){
			return false;
		}

		if(!plots[y][x].open){
			//plot isn't open, can't build there
			return false;
		}

		//the room can not be built overtop nothing
		if(!plots[y][x].ground && checkBelow){
			if(!plots[y - 1][x].BuildOnTop(room)){
				return false;
			}
		}
		
		return true;
	}

	public static void ZoomIn(){
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		//Debug.Log(cam.orthographicSize);
		if(cam.orthographicSize > 2){
			cam.orthographicSize--;
			//need to adjust the Y axis of the camera while zooming to keep focus on the center point
			cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y - .5f, cam.transform.localPosition.z);
		}

		orthoSize = (int)cam.orthographicSize;

		CheckCameraYMin();
		CheckCameraYMax();
		CheckCameraXMin();
		CheckCameraXMax();
	}
	
	public static void ZoomOut(){
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		
		//Debug.Log(cam.orthographicSize);
		if(cam.orthographicSize < 8){
			cam.orthographicSize++;
			//need to adjust the Y axis of the camera while zooming to keep focus on the center point
			cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y + .5f, cam.transform.localPosition.z);
		}

		orthoSize = (int)cam.orthographicSize;

		CheckCameraYMin();
		CheckCameraYMax();
		CheckCameraXMin();
		CheckCameraXMax();
	}	
	
	public static void CheckCameraYMin()
	{
		float cap = orthoSize - 3;
		
		if(Camera.main.transform.position.y < cap){
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cap, -10);
		}
	}

	public static void CheckCameraYMax()
	{
		float cap = World.orthoSize - 4 + World.HEIGHT;
		
		if(cap < World.orthoSize - 3){
			cap = World.orthoSize - 3;
		}
		
		if(Camera.main.transform.position.y > cap){
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cap, -10);
		}
	}

	public static void CheckCameraXMin()
	{

		float cap = World.orthoSize;
		
		if(Camera.main.transform.position.x < cap){
			Camera.main.transform.position = new Vector3(cap, Camera.main.transform.position.y, -10);
		}
	}

	public static void CheckCameraXMax()
	{

		float cap = World.orthoSize + World.WIDTH - 12;

		if(cap < World.orthoSize){
			cap = World.orthoSize;
		}
		
		if(Camera.main.transform.position.x > cap){
			Camera.main.transform.position = new Vector3(cap, Camera.main.transform.position.y, -10);
		}
	}

    // For pathfinding
    public static void ConstructGraph() {
		pathfindingGraph.ClearConnections();
        for (int i = 0; i < HEIGHT; i++) {
            for (int k = 0; k < WIDTH - 1; k++) {
                //For all nodes on the ground floor.
                if (plots[i][k].ground) {
                    pathfindingGraph.AddConnection(new Connection(new Vector2(k, i), new Vector2(k + 1, i), 1));
                    Debug.Log("Horizontal Ground connection: (" + k + ", " + i + ") to (" + (k + 1) + ", " + i + ").");
                } else {
                    //For non-grounded rooms
                    if (plots[i][k].GetRoom() != null) {
                        //For rooms which have a ladder connecting them the the previous floor.
                        if (plots[i - 1][k].GetRoom().hasLadder) {
                            if ((int) plots[i-1][k].GetRoom().verticalTransportObject.transform.position.x == k) {
                                pathfindingGraph.AddConnection(new Connection(new Vector2(k, i), new Vector2(k, i - 1), 1));
                                Debug.Log("Ladder connection: (" + k + ", " + i + ") to (" + k + ", " + (i - 1) + ").");
                            }
                        }
                        //For rooms which have not been connected yet.
                        if (plots[i][k + 1].GetRoom() != null) {
                            pathfindingGraph.AddConnection(new Connection(new Vector2(k, i), new Vector2(k + 1, i), 1));
                            Debug.Log("Horizontal connection: (" + k + ", " + i + ") to (" + (k + 1) + ", " + i + ").");
                        }
                    }
                }


            }
        }
    }

    struct NodeRecord {
        public Vector2 node;
        public Connection connection;
        public float costSoFar;

        public static NodeRecord GetNextNodeIteration(List<NodeRecord> list) {
            NodeRecord nextNode = list[0];
            float smallestCostSoFar = list[0].costSoFar;
            for (int i = 1; i < list.Count; i++) {
                if (list[i].costSoFar < smallestCostSoFar) {
                    smallestCostSoFar = list[i].costSoFar;
                    nextNode = list[i];
                }
            }
            Debug.Log("Next Node To Be Analysed: (" + nextNode.node.x + ", " + nextNode.node.y + ").");
            return nextNode;
        }
    }

    public static List<Vector2> FindPath(Vector2 startNode, Vector2 goalNode) {
        List<NodeRecord> openNodes = new List<NodeRecord>();
        List<NodeRecord> closedNodes = new List<NodeRecord>();

        NodeRecord start = new NodeRecord();
        NodeRecord current = new NodeRecord();
        start.node = startNode;
        start.connection = null;
        start.costSoFar = 0;
        openNodes.Add(start);

        while (openNodes.Count > 0) {
            current = NodeRecord.GetNextNodeIteration(openNodes);
            List<Connection> nodeConnections;
            if (current.node == goalNode) {
                break;
            }
            nodeConnections = pathfindingGraph.GetConnections(current.node);
            foreach (Connection c in nodeConnections) {
                NodeRecord endNodeRecord;
                Vector2 endNode = c.OtherNode(current.node);
                float endNodeCost = current.costSoFar + c.Weight;
                if (closedNodes.Exists(o => o.node == endNode)) {
                    continue;
                } else if (openNodes.Exists(o => o.node == endNode)) {
                    endNodeRecord = openNodes.Find(o => o.node == endNode);
                    if (endNodeRecord.costSoFar <= endNodeCost) {
                        continue;
                    }
                } else {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                }
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = c;
                if (!openNodes.Exists(o => o.node == endNode)) {
                    openNodes.Add(endNodeRecord);
                }
            }
            openNodes.Remove(current);
            closedNodes.Add(current);
        }

        if (current.node != goalNode) {
            return null;
        } else {
            List<Vector2> path = new List<Vector2>();
            while (current.node != startNode) {
                path.Add(current.node);
                current = closedNodes.Find(o => o.node == current.connection.OtherNode(current.node));
            }
            path.Reverse();
            return path;
        }
    }

    public static void initialiseLayers()
    {
        //private int citizenGrounded = 9;
        //private int ascendLayer = 14;
        //private int descendLayer = 15;
        //private int platformLayer = 19;
        //private int findLadder = 21;

        //Grounded Layer Settings
        /*Physics2D.IgnoreLayerCollision(groundedLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(8, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(groundedLayer, 10, true);
        Physics2D.IgnoreLayerCollision(groundedLayer, 11, true);
        Physics2D.IgnoreLayerCollision(groundedLayer, 18, true);

        //Ascend Ladder 1 Layer Settings
        Physics2D.IgnoreLayerCollision(ascendLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, ascendLayer, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, 8, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, 10, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, 11, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, 11, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, 18, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, landingLayer, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, platformLayer, true);
        Physics2D.IgnoreLayerCollision(ascendLayer, findLadderLayer, true);

        //Descend Ladder 1 Layer Settings
        Physics2D.IgnoreLayerCollision(descendLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, ascendLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, 8, true);
        Physics2D.IgnoreLayerCollision(descendLayer, 10, true);
        Physics2D.IgnoreLayerCollision(descendLayer, ladderLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, findLadderLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, 20, true);
        Physics2D.IgnoreLayerCollision(descendLayer, 11, true);
        Physics2D.IgnoreLayerCollision(descendLayer, 18, true);
        Physics2D.IgnoreLayerCollision(descendLayer, landingLayer, true);
        Physics2D.IgnoreLayerCollision(descendLayer, platformLayer, true);

        //Land Ladder Descent Settings
        Physics2D.IgnoreLayerCollision(landingLayer, 22, true);
        Physics2D.IgnoreLayerCollision(landingLayer, ascendLayer, true);
        Physics2D.IgnoreLayerCollision(landingLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(landingLayer, 8, true);
        Physics2D.IgnoreLayerCollision(landingLayer, 10, true);
        Physics2D.IgnoreLayerCollision(landingLayer, ladderLayer, true);
        Physics2D.IgnoreLayerCollision(landingLayer, findLadderLayer, true);
        Physics2D.IgnoreLayerCollision(landingLayer, 20, true);
        Physics2D.IgnoreLayerCollision(landingLayer, 11, false);
        Physics2D.IgnoreLayerCollision(landingLayer, 18, true);
        Physics2D.IgnoreLayerCollision(landingLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(landingLayer, platformLayer, true);

        //2nd Floor or higher layer settings.
        Physics2D.IgnoreLayerCollision(platformLayer, platformLayer, true);
        Physics2D.IgnoreLayerCollision(platformLayer, ascendLayer, true);
        Physics2D.IgnoreLayerCollision(platformLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(platformLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(8, platformLayer, true);
        Physics2D.IgnoreLayerCollision(platformLayer, 10, true);
        Physics2D.IgnoreLayerCollision(platformLayer, 11, true);
        Physics2D.IgnoreLayerCollision(platformLayer, ladderLayer, true);
        Physics2D.IgnoreLayerCollision(platformLayer, 11, false);
        Physics2D.IgnoreLayerCollision(platformLayer, 18, true);
        Physics2D.IgnoreLayerCollision(platformLayer, 9, true);

        //On platform ascend Layer, looks for ascending ladder
        Physics2D.IgnoreLayerCollision(20, 20, true);
        Physics2D.IgnoreLayerCollision(20, findLadderLayer, true);
        Physics2D.IgnoreLayerCollision(20, platformLayer, true);
        //Changes this relationship to false
        Physics2D.IgnoreLayerCollision(20, ascendLayer, false);
        Physics2D.IgnoreLayerCollision(20, descendLayer, true);
        Physics2D.IgnoreLayerCollision(8, 20, true);
        Physics2D.IgnoreLayerCollision(20, 10, true);
        Physics2D.IgnoreLayerCollision(20, 11, true);
        Physics2D.IgnoreLayerCollision(20, ladderLayer, true);
        Physics2D.IgnoreLayerCollision(20, 11, false);
        Physics2D.IgnoreLayerCollision(20, 18, true);

        //On platform descend Layer, looks for decending ladder
        Physics2D.IgnoreLayerCollision(findLadderLayer, findLadderLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, 20, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, platformLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, ascendLayer, true);
        //Changes this relationship to false
        Physics2D.IgnoreLayerCollision(findLadderLayer, descendLayer, true);
        Physics2D.IgnoreLayerCollision(8, findLadderLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, landingLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, 10, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, 11, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, groundedLayer, true);
        Physics2D.IgnoreLayerCollision(findLadderLayer, ladderLayer, false);
        Physics2D.IgnoreLayerCollision(findLadderLayer, 11, false);*/
    }
}
