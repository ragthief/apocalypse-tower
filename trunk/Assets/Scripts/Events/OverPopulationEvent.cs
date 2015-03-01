using UnityEngine;
using System.Collections;

public class OverPopulationEvent : MonoBehaviour {

	// Each event should have at least an event object of a delegate, an add method procedure, remove method procedure
	private event OverPopulationDel overPopulationEvent;

	public void AddMethod(OverPopulationDel method) {
		overPopulationEvent += method;
	}
	
	public void RemoveMethod(OverPopulationDel method) {
		overPopulationEvent -= method;
	}

	// I think it would be a good idea to document where the update method for each event is called from.
	/*
		Called from:
		Tower.AddRoom
		Tower.DeleteRoom
		Tower.addCitizen
	*/
	public void Update() {
		if (!Tower.IsOverPopulated && Tower.getCurrentPopulation() >= Tower.getPopulationCap() * 0.95 && Tower.getCurrentPopulation() != 0) {
			Tower.IsOverPopulated = true;
			overPopulationEvent (Tower.IsOverPopulated);
		} else if (Tower.IsOverPopulated && Tower.getCurrentPopulation() < Tower.getPopulationCap() * 0.95) {
			Tower.IsOverPopulated = false;
			overPopulationEvent (Tower.IsOverPopulated);
		}
	}
}