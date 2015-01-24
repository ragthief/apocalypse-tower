using UnityEngine;
using System.Collections;

public class LeftBuildingEdge : MonoBehaviour {
    public int numberRoomCollisions = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("BuildingEdge");
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "RightBuildingEdge")
        {
            Debug.Log("Collision");
        }
    }
}