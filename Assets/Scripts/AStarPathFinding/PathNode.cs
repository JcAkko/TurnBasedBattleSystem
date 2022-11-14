using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the node object that stored inside the path finding grid system
// this object stores to cost info for each path finding
public class PathNode
{
    // record the grid positon of this node object
    private GridPosition gridPosition;

    // the path costs
    private int gCost;
    private int hCost;
    private int fCost;

    // store the pathNode where this Node cam from
    private PathNode cameFromPathNode;

    // constructor
    public PathNode(GridPosition gridPos_)
    {
        // sign the grid position of this node
        this.gridPosition = gridPos_;
    }


    // override the to string to expose the grid x,z
    public override string ToString()
    {
       
        // return the grid postion of this node
        return gridPosition.ToString();
    }


    // expose the cost
    public int GetGCost()
    {
        return gCost;
    }

    // expose the cost
    public int GetHCost()
    {
        return hCost;
    }

    // expose the cost
    public int GetFCost()
    {
        return fCost;
    }


}
