using UnityEngine;
using System.Collections;

public class Connection {

    private Vector2 nodeOne, nodeTwo;
    private float weight;

    public Connection(Vector2 nodeOne, Vector2 nodeTwo, float weight) {
        this.nodeOne = nodeOne;
        this.nodeTwo = nodeTwo;
        this.weight = weight;
    }

    public Vector2 OtherNode(Vector2 knownNode) {
        if (knownNode == nodeOne) {
            return nodeTwo;
        } else {
            return nodeOne;
        }
    }

    public Vector2 NodeOne {
        get { return nodeOne; }
    }

    public Vector2 NodeTwo {
        get { return nodeTwo; }
    }

    public float Weight {
        get { return weight; }
    }
}
