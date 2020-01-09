using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// An implementation of A* for the new TileMap system released by Unity
/// </summary>
public class Pathfinding : MonoBehaviour {
    public CreateNodesFromTilemaps _grid;

    private static Pathfinding _pathfinding;
    public static Pathfinding instance {
        get {
            if (_pathfinding == null) _pathfinding = FindObjectOfType<Pathfinding> ();
            return _pathfinding;
        }
    }

    void Awake () {
        _grid = FindObjectOfType<CreateNodesFromTilemaps> ();
    }
    public bool FindPath (Vector3Int start, Vector3Int target, out List<Node> path) {
        start.Clamp (new Vector3Int (0, 0, 0), new Vector3Int (300, 300, 0));
        target.Clamp (new Vector3Int (0, 0, 0), new Vector3Int (300, 300, 0));
        path = new List<Node> ();
        Node startNode = _grid.GetNode (new Vector2 (start.x, start.y));
        Node targetNode = _grid.GetNode (new Vector2 (target.x, target.y));

        List<Node> openSet = new List<Node> ();
        HashSet<Node> closedSet = new HashSet<Node> ();
        if (!targetNode.walkable || startNode == null || targetNode == null)
            return false;

        openSet.Add (startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];

            openSet.Remove (currentNode);
            closedSet.Add (currentNode);

            if (currentNode.Equals (targetNode)) {
                path = RetracePath (startNode, currentNode);
                return true;
            }

            foreach (Node neighbour in currentNode.myNeighbours) {
                if (!neighbour.walkable || closedSet.Contains (neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance (neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains (neighbour)) {
                        openSet.Add (neighbour);
                        openSet = openSet.OrderBy (a => a.fCost).ToList ();
                    }
                }
            }

            openSet = openSet.OrderBy (a => a.fCost).ToList ();
        }
        return false;
    }

    private List<Node> RetracePath (Node startNode, Node endNote) {
        List<Node> path = new List<Node> ();
        Node currentNode = endNote;

        while (!currentNode.Equals (startNode)) {
            path.Add (currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse ();
        return path;
    }

    private int GetDistance (Node a, Node b) {
        int dstX = Mathf.Abs (a.gridX - b.gridX);
        int dstY = Mathf.Abs (a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}