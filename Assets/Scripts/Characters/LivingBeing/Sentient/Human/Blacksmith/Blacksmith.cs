using UnityEngine;
using System.Collections;

public class Blacksmith : Human {
    public int gatheredMetal = 0;
    public int processedMetal = 0;

    public virtual void gatherWood(int metalToGather) {
        gatheredMetal += metalToGather;
    }

    public virtual void processMetal(int metalToProcess) {
        processedMetal += metalToProcess;
    }
}
