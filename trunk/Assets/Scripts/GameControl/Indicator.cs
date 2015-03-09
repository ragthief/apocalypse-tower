using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {
    public static int numIndicators = 0;
    public static bool destroyOld = false;
    public static int indicatorPopulation = 1;
    public bool imOld = false;
    public bool imNewb = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (imNewb)
        {
            imNewb = false;
            numIndicators++;
            destroyOld = true;
        }
        if (numIndicators > indicatorPopulation)
        {
            if (destroyOld && imOld)
            {
                Destroy(this.gameObject);
                if (numIndicators > 0)
                {
                    numIndicators--;
                }
            }
            if(indicatorPopulation == numIndicators) {
                indicatorPopulation = 1;
            }
        }
        else
        {
            destroyOld = false;
            imOld = true;
        }
	}

    public static void DestroyAll()
    {
        destroyOld = true;
        indicatorPopulation = 0;
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
        numIndicators--;
    }
}
