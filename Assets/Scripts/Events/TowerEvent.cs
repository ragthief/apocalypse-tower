using UnityEngine;
using System.Collections;

public delegate void OverPopulationDel(bool overPopulated);
//public delegate eventMethodSignature;

public class TowerEvent : MonoBehaviour {

	public static OverPopulationEvent overPopulation;
	//public static eventObjects;

	void Awake() {
		overPopulation = new OverPopulationEvent();
	}

	void Start () {
		overPopulation.AddMethod(OverPopulation);
	}

	// Temporary method added to the event in the start method for testing. Called when event begins and ends.
	public void OverPopulation(bool overPopulated) {
		Debug.Log("Over Populated? " + overPopulated);
	}

	// Another temporary method to display a message to the screen. Can be moved to GUI class.
	void OnGUI() {
		if (Tower.IsOverPopulated) {
			GUI.Label (new Rect (100, 100, 300, 50), "Over Populated!");
		}
	}
}