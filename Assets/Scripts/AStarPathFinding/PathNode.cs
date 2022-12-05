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

    // whether this node is walkable or not
    private bool isWalkable = true;

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


    // function used to set the gcost
    public void SetGCost(int gCost_)
    {
        gCost = gCost_;
    }

    // function used to set the hcost
    public void SetHCost(int hCost_)
    {
        hCost = hCost_;
    }

    // function used to calculate the fcost
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    // reset the cam from pathnode
    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    // set the cam from pathnode
    public void SetCameFromPathNode(PathNode pathNode_)
    {
        cameFromPathNode = pathNode_;
    }

    // get the cam from pathnode
    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    // expose the gridposition
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // expose if the node is walkable
    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetNodeIsWalkAble(bool isWalkAble_)
    {
        this.isWalkable = isWalkAble_;
    }


    // used to check if one position is already occupied by any unit
    public bool IsOccupiedByAnyUnit()
    {
        return LevelGrid.Instance.IsGridPostionOccupied(gridPosition);
    }


}
