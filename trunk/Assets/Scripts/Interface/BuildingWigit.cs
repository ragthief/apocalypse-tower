using UnityEngine;
using System.Collections;

public class BuildingWigit : MonoBehaviour {

	private bool open = false;
	public enum TYPE { ROOM };
	public TYPE type = TYPE.ROOM;
	public float rollIndex = 0;
	public float rollSpeed = .1f;
	public float rollAmount = 0;
	private Texture2D roomSnaps;
	protected string[] options = new string[]{};
	public gameGUI owner;
	private GameObject placingRoom = null;
    private bool isPlaced = false;
    private string filePath = "Prefabs/RoomPrefabs/";

	void OnGUI()
	{
		if(open){
			float angleStep = Mathf.PI*2f / options.Length;
			Vector2 circleCenter = new Vector2(Screen.width/2 + 25,Screen.height);
			float radius = Screen.width / 4;
			for (int i = 0; i < options.Length; i++)
			{
				Rect R = new Rect(0,0,100,120); // adjust the size
				R.x = circleCenter.x + Mathf.Cos(angleStep*(i + rollIndex - Mathf.Floor(options.Length / 4)))*radius * 1.9f - R.width/2;
				R.y = circleCenter.y + Mathf.Sin(angleStep*(i + rollIndex - Mathf.Floor(options.Length / 4)))*radius * .75f - R.height/2;
				float scale = (circleCenter.y + radius - R.y + Screen.width / 4f) / (circleCenter.y + radius);
				scale = scale * scale;
				GUI.BeginGroup(new Rect(R.x, R.y, R.width * scale, R.height * scale));
				//only allow the click interaction if the button is far enough on screen, 
				//because the half circle overlaps the buttons near the corner of the screen
				GUI.enabled = false;

				if(R.x > Screen.width * .02 && R.x < Screen.width * .92){
					GUI.enabled = true;
				}

				if (GUI.Button(new Rect(0, 0, 100 * scale, 120 * scale),""))                                                                    
				{
                    placingRoom = Instantiate(Resources.Load(filePath + options[i]), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
					Toggle(type);
				}
				GUI.DrawTexture(new Rect(-i*100 * scale, 0, 2200 * scale, 100 * scale), roomSnaps);
				GUI.skin.label.alignment = TextAnchor.UpperCenter;
				GUI.Label(new Rect(0, 100 * scale, 100 * scale, 30* scale), options[i]);
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.EndGroup();
				GUI.enabled = true;
			}
		}
	}

    public static void roomIsPlaced(bool roomPlaced)
    {

    }

	// Use this for initialization
	void Start () {
		roomSnaps = (Texture2D)Resources.Load("Sprites/GameControl/RoomSnaps");
	}
	
	// Update is called once per frame
	void Update () {
		if(open){
			if(rollAmount < 0){
				rollIndex += rollSpeed;
				rollAmount += rollSpeed;
			}else if(rollAmount > 0){
				rollIndex -= rollSpeed;
				rollAmount -= rollSpeed;
			}
			if(Mathf.Abs(rollAmount) < rollSpeed){
				rollAmount = 0;
			}
			CheckRollOver();

			//scroll wheel controls
			rollAmount += (Input.GetAxis("Mouse ScrollWheel")) * 3;

			//keyboard controls
			if (Input.GetKey(KeyCode.LeftArrow)) {
				rollAmount -= .1f;
			}
			if (Input.GetKey(KeyCode.RightArrow)) {
				rollAmount += .1f;
			}
		}

		//keyboard controls to open wigit
		if (Input.GetKeyDown(KeyCode.B)) {
			Toggle(TYPE.ROOM);
			if(placingRoom != null){
				GameObject.Destroy(placingRoom);
				placingRoom = null;
			}
		}
	}

	public void CheckRollOver(){

		switch(type){
		case TYPE.ROOM:
			if(rollIndex < 0){
				rollIndex = options.Length + rollIndex;
			}else if(rollIndex >= options.Length){
				rollIndex = options.Length - rollIndex;
			}
			break;
		}
	}

    public string getRoom(int i)
    {
        string roomType = "null";
        return roomType;
    }

	public void Toggle(TYPE type){
		if(this.type != type)
		{
			rollIndex = 0;
			this.type = type;
		}

		switch(this.type){
		case TYPE.ROOM:
			this.options = Room.AvailableRooms();
			break;
		}

		open = !open;
	}
}
