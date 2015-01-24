using UnityEngine;
using System.Collections;

public class BuildingEdge : MonoBehaviour {
    public int numberRoomCollisions = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("BuildingEdge");
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("BuildingCollision");
        Destroy(this);
    }
}
