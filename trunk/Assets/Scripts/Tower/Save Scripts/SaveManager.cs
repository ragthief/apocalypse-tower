using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

public class SaveManager {

	static string path = ".\\saves\\";
	static string ext = ".sv";

	public static void SaveGame(SavePacket packet){
		XmlSerializer x = new XmlSerializer(packet.GetType());
		
		StringWriter writer = new StringWriter();
		
		x.Serialize(writer, packet);
		
		//create directories if needed
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);
		
		System.IO.StreamWriter file = new System.IO.StreamWriter(path + packet.towerName + ext);
		file.Write(writer.ToString());
		file.Close();
	}

	public static SavePacket LoadGame(string name){
		SavePacket packet = new SavePacket();
		
		List<string> saveFiles = LoadSaveFileNames();

		foreach (string file in saveFiles)
		{
			if(file == name + ext)
			{
				StreamReader reader = new StreamReader(file);
				string line;
				string str = "";
				while ((line = reader.ReadLine()) != null)
				{
					str += line;
				}
				reader.Close();
				
				XmlSerializer x = new XmlSerializer(packet.GetType());
				
				TextReader tReader = new StringReader(str);
				
				packet = x.Deserialize(tReader) as SavePacket;
			
			}
		}

		return packet;
	}

	public static List<string> LoadSaveFileNames(){
		List<string> saveNames = new List<string>();
		
		saveNames = Directory.GetFiles(path, "*" + ext).ToList();

		return saveNames;
	}

	public static void DeleteSave(string name){

	}
}
