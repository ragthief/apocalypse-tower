using UnityEngine;
using System.Collections;

public class RoomObject : MonoBehaviour {

	public Vector2 position = new Vector2(0, 0);
	protected bool on = false, powered = false;
	public bool usesPower = false;
	public float powerConsumption = 0.0f;

	public Room owner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public float ConsumePower(float units){
		if(on && units >= powerConsumption){
			powered = true;
		}else {
			powered = false;
		}

		if(on){
			units -= powerConsumption;
			if(units < 0){
				units = 0f;
			}
		}

		return units;
	}

	public void TurnOn(){
		if(usesPower){
			this.on = true;
		}
	}
	public void TurnOff(){
		if(usesPower){
			this.on = false;
		}
	}
	public bool On{
		get { return on; }
	}
	public bool Powered{
		get { return powered; }
	}
}
