using UnityEngine;
using System.Collections;

public class Node {

	//for building on
	public bool open = true;

	//is this the ground or the world?
	public bool ground = false;

	//store the building built on top of it
	private Room room = null;

	public void BuildRoom(Room room){
		this.room = room;
		open = false;
	}

	public void Clear(){
		open = true;
		room = null;
	}

	public bool BuildOnTop(Room room){
		if(this.room == null){
			return false;
		}
		return this.room.BuildOnTop(room);
	}

    public Room GetRoom() {
        return room;
    }
}
