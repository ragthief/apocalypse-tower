using UnityEngine;
using System.Collections;

public class PrefabPicker {

    // This code will have to be modified in order for the rooms to be initialised correctly, due to the amended tower and room code.

	/*public static Room CreateRoomPrefab(Tower t){
		GameObject room = MonoBehaviour.Instantiate(Resources.Load("Room"), new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		Room roomScript = room.GetComponent<Room>();
		return(roomScript);
	}

	public static RoomObject CreateRoomObjectPrefab(string name){
		GameObject roomObject = MonoBehaviour.Instantiate(Resources.Load(name), new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		RoomObject roScript = roomObject.GetComponent<RoomObject>();
		return(roScript);
	}

	public static PowerSource CreatePowerSourcePrefab(){
		GameObject powerSource = MonoBehaviour.Instantiate(Resources.Load("PowerSource"), new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		PowerSource psScript = powerSource.GetComponent<PowerSource>();
		return(psScript);
	}*/
}
