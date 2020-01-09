using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


public class Node : MonoBehaviour
{
    public int gCost;
    public int hCost;
    public int gridX, gridY;
    public bool walkable = true;
    public List<Node> myNeighbours;
    public Node parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    
    public Vector3 vectorPos{
        get{
            return new Vector3(gridX,gridY);
        }
    }

    public override bool Equals(object obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Node nodeObj = (Node)obj;
        if(nodeObj.vectorPos == vectorPos){
            return true;
        }        

        return base.Equals (obj);
    }
    
}