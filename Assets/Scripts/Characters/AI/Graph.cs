using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph {

    private List<Connection> connections = new List<Connection>();

    public List<Connection> GetConnections(Vector2 node) {
        List<Connection> connectionForNode = new List<Connection>();
        foreach (Connection c in connections) {
            if (c.NodeOne == node || c.NodeTwo == node) {
                connectionForNode.Add(c);
            }
        }
        return connectionForNode;
    }

    public void AddConnection(Connection c) {
        connections.Add(c);
    }

    public void ClearConnections() {
        connections.Clear();
    }
}
