using UnityEngine;
using System.Collections;

public class GenerateName : MonoBehaviour {
    public static string[] maleNames = new string[4];
    public static string[] femaleNames = new string[4];

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void initNames()
    {
        maleNames[0] = "Joe";
        maleNames[1] = "Bill";
        maleNames[2] = "Bob";
        maleNames[3] = "Vlad the Impaler";

        femaleNames[0] = "Ashley";
        femaleNames[1] = "Lisa";
        femaleNames[2] = "Tina";
        femaleNames[3] = "LaShondra";
    }

    public static string getName(char sex)
    {
        //If male name required, provide male name
        if (sex == 'm')
        {
            int rInt = Random.Range(0, 3);
            return maleNames[rInt];     
        }
        //If female name required, provide female name
        else
        {
            int rInt = Random.Range(0, 3);
            return femaleNames[rInt];
        }
    }
}
