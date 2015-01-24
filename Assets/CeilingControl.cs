using UnityEngine;
using System.Collections;

public class CeilingControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("CeilingCollision");
        if (col.gameObject.tag == "Floor" && ((col.transform.position.x - this.transform.position.x)>1f || (this.transform.position.x - col.transform.position.x)>1f))
        {
            Destroy(this);
            Debug.Log("CeilingDestroyed");
        }
    }
}
