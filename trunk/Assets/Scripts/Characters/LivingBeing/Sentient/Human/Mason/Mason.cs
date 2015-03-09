using UnityEngine;
using System.Collections;

public class Mason : Human {
    public int gatheredStones = 0;
    public int processedStones = 0;

    public virtual void gatherStones(int stonesToGather) {
        gatheredStones += stonesToGather;
    }

    public virtual void processStones(int stonesToProcess) {
        processedStones += stonesToProcess;
    }
}
