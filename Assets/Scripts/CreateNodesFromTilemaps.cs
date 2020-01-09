using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CreateNodesFromTilemaps : MonoBehaviour {
	//did some stuff to the actions in npc so they can get closer to the Nodes without the glitchyness

	//changed execution order for this and world builder
	public Grid gridBase;
	public Tilemap floor; //floor of world
	public List<Tilemap> obstacleLayers; //all layers that contain objects to navigate around
	public GameObject nodePrefab;
	public Transform nodeParent;
	//these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
	public int scanStartX = -250, scanStartY = -250, scanFinishX = 250, scanFinishY = 250;

	public List<GameObject> unsortedNodes; //all the nodes in the world

	public Dictionary<Vector2, Node> nodes = new Dictionary<Vector2, Node> ();
	//public GameObject[, ] nodes; //sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps
	public Vector2 gridBoundMin, gridBoundMax;
	private static CreateNodesFromTilemaps _nodeMaker;
	public static CreateNodesFromTilemaps Instance {
		get {
			if (_nodeMaker == null) _nodeMaker = FindObjectOfType<CreateNodesFromTilemaps> ();
			return _nodeMaker;
		}
	}
	void Awake () {
		unsortedNodes = new List<GameObject> ();
		CreateNodes();
	}

	public void CreateNodes () {
		int gridX = 0;
		int gridY = 0;

		//scan tiles and create nodes based on where they are
		for (int x = scanStartX; x < scanFinishX; x++) {
			for (int y = scanStartY; y < scanFinishY; y++) {
				//go through our world bounds in increments of 1
				TileBase tb = floor.GetTile (new Vector3Int (x, y, 0)); //check if we have a floor tile at that world coords
				if (tb == null) { } else {
					//if we do we go through the obstacle layers and check if there is also a tile at those coords if so we set founObstacle to true
					bool foundObstacle = false;
					foreach (Tilemap t in obstacleLayers) {
						TileBase tb2 = t.GetTile (new Vector3Int (x, y, 0));

						if (tb2 != null) {
							foundObstacle = true;
						}

						//if we want to add an unwalkable edge round our unwalkable nodes then we use this to get the neighbours and make them unwalkable
						if (unwalkableNodeBorder > 0) {
							List<TileBase> neighbours = getNeighbouringTiles (x, y, t);
							foreach (TileBase tl in neighbours) {
								if (tl == null) {

								} else {
									foundObstacle = true;
								}
							}
						}
					}
					GameObject node = (GameObject) Instantiate (nodePrefab, new Vector3 (x + gridBase.transform.position.x, y + gridBase.transform.position.y, 0), Quaternion.Euler (0, 0, 0), nodeParent);
					Node wt = node.GetComponent<Node> ();
					wt.gridX = gridX = (int) node.transform.position.x;
					wt.gridY = gridY = (int) node.transform.position.y;
					unsortedNodes.Add (node);

					if (foundObstacle == false) {
						//if we havent found an obstacle then we create a walkable node and assign its grid coords
						node.name = "NODE " + wt.gridX.ToString () + " : " + wt.gridY.ToString ();
					} else {
						//if we have found an obstacle then we do the same but make the node unwalkable
						wt.walkable = false;
						node.name = "UNWALKABLE NODE " + wt.gridX.ToString () + " : " + wt.gridY.ToString ();
					}

					if (gridX > gridBoundMax.x) {
						gridBoundMax.x = gridX;
					}

					if (gridY > gridBoundMax.y) {
						gridBoundMax.y = gridY;
					}
					if (gridX < gridBoundMin.x) {
						gridBoundMin.x = gridX;
					}

					if (gridY < gridBoundMin.y) {
						gridBoundMin.y = gridY;
					}
				}
			}
		}

		//put nodes into 2d array based on the 
		//nodes = new GameObject[gridBoundX + 1, gridBoundY + 1)]; //initialise the 2d array that will store our nodes in their position 
		foreach (GameObject g in unsortedNodes) { //go through the unsorted list of nodes and put them into the 2d array in the correct position
			Node wt = g.GetComponent<Node> ();
			//Debug.Log (wt.gridX + " " + wt.gridY);
			nodes[wt.vectorPos] = wt;
		}

		//assign neighbours to nodes
		for (int x = (int) gridBoundMin.x; x < gridBoundMax.x; x++) { //go through the 2d array and assign the neighbours of each node
			for (int y = (int) gridBoundMin.y; y < gridBoundMax.y; y++) {
				if (NodeExists(new Vector2 (x, y))) { //check if the coords in the array contain a node
					Node wt = GetNode(new Vector2 (x, y)); //if they do then assign the neighbours
					//if (wt.walkable == true) {
					//wt.myNeighbours = getNeighbours (x, y, gridBoundX, gridBoundY);
					wt.myNeighbours = getNeighbours (wt);
					//}
				}
			}
		}

		//after this we have our grid of nodes ready to be used by the astar algorigthm

	}

	//gets neighbours of a tile at x/y in a specific tilemap, can also have a border
	public int unwalkableNodeBorder = 1;
	public List<TileBase> getNeighbouringTiles (int x, int y, Tilemap t) {
		List<TileBase> retVal = new List<TileBase> ();

		for (int i = x - unwalkableNodeBorder; i < x + unwalkableNodeBorder; i++) {
			for (int j = y - unwalkableNodeBorder; j < y + unwalkableNodeBorder; j++) {
				TileBase tile = t.GetTile (new Vector3Int (i, j, 0));
				if (tile == null) {

				} else {
					retVal.Add (tile);
				}
			}
		}

		return retVal;
	}
	public List<Node> getNeighbours (Node node) {
		List<Node> neighbours = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX > gridBoundMin.x && checkX < gridBoundMax.x && checkY > gridBoundMin.y && checkY < gridBoundMax.y) {
					if (NodeExists(new Vector2 (checkX, checkY)))
						neighbours.Add (GetNode(new Vector2 (checkX, checkY)));
				}
			}
		}

		return neighbours;
	}
	public bool NodeExists (Vector2 v) {
		if (nodes.ContainsKey (v)) {
			return true;
		}
		return false;
	}
	public Node GetNode (Vector2 v) {
		if (nodes.ContainsKey (v)) {
			return nodes[v];
		}
		return null;
	}
}