using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavePacket {

	//this class will only have public attributes that we want to save off
	public string towerName = "test";
	public List<Node> nodes = new List<Node>();
}
