using UnityEngine;
using System.Collections;

public class Lumberjack : Human {
    public int gatheredWood = 0;
    public int processedWood = 0;

    public virtual void gatherWood(int woodToGather) {
        gatheredWood += woodToGather;
    }

    public virtual void processWood(int woodToProcess) {
        processedWood += woodToProcess;
    }
}
