using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this game use A star path finding algorithm
// the path finding system will utilize the same grid system base as the levelGrid do, but a differetn gridsystem Type
// run before any other script in the editor
public class PathFinding : MonoBehaviour
{

    // expose the class by make an static instance of the class
    public static PathFinding Instance { get; private set; }

    // prefab for debug object
    [SerializeField] private Transform gridDebugObjectPrefab;

    // define the width, height and cellsize of the path finding grids
    // the length of the map on x axis
    private int xlength;

    // the length of the map on z axis
    private int zlength;

    // the size of each individual floor cell
    private float cellSize;

    // store the current path finding grid system
    private GridSystem<PathNode> pfGridSystem;

    // the cost for moving straight
    private const int move_Straight_Cost = 10;
    // the cost for moving diagonol
    // square root of 2
    private const int move_Diagonal_Cost = 14;


    // layer mask for the obstacles
    [SerializeField] private LayerMask obstacleLayerMask;

    private void Awake()
    {
        // check if there's multiple instance for this class, if so, destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }
        // if not instance yet, sign the instance
        Instance = this;

        // **** Path finding grid system will be populated inside the level grid script

    }

    // make connection to the level grid so the size of the path finding grid system will be the same as the level grid system
    public void PFGridSystemSetUp(int xLength_, int zLength_, float cellSize_)
    {
        this.xlength = xLength_;
        this.zlength = zLength_;
        this.cellSize = cellSize_;

        // create the grid system for path finding
        pfGridSystem = new GridSystem<PathNode>(xlength, zlength, cellSize,
            (GridSystem<PathNode> g_, GridPosition gridPosition_) => new PathNode(gridPosition_));

        // populate the gridDebugObjects onto each grid
        pfGridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        // cycle through all the grid positions and location all the pathnodes that occupied by the obstacles
        // set them all as unwalkable
        for (int x = 0; x < xlength; x++)
        {
            for (int z = 0; z < zlength; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                // get the world pos of this grid position
                Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(gridPosition);
                //ray cast to the obstacles layer see it blocked
                // to prevent the raycast blocked by the obstacle, lower the cast position by somedistance on y
                float rayCastOffSetDistance = 5.0f;
                // if there is an obstacle, then set the node at this position as unwalkable
                if (Physics.Raycast(worldPos + Vector3.down * rayCastOffSetDistance, Vector3.up, rayCastOffSetDistance * 2, obstacleLayerMask))
                {
                    GetPathNodeAtGridPosition(x, z).SetNodeIsWalkAble(false);
                }
            }
        }
    

    }


    // function used to find the path from one node to another
    public List<GridPosition> FindPath(GridPosition startPos_, GridPosition endPos_)
    {
        // create open list and the close list
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        // find the start node
        PathNode startNode = pfGridSystem.GetGridObject(startPos_);
        // find the end node
        PathNode endNode = pfGridSystem.GetGridObject(endPos_);

        // add start node into the open list
        openList.Add(startNode);

        // cycle through all the nodes inside the system and reset their state
        for (int x = 0; x < pfGridSystem.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < pfGridSystem.GetGridSystemHeight(); z++)
            {
                GridPosition gridPos = new GridPosition(x, z);
                // get the path node object at this grid position
                PathNode pathNode = pfGridSystem.GetGridObject(gridPos);

                // reset the gcost and Hcost for the node
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                // reset the previous node history
                pathNode.ResetCameFromPathNode();
            }
        }

        // calculate the G H and F cost for the start node
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPos_, endPos_));
        startNode.CalculateFCost();

        // while there is still node inside the open list, calculate all of them
        while (openList.Count > 0)
        {
            // alway grab to one in the openlist with lowest F cost
            PathNode currentNode = GetLowestFCostNodeFromList(openList);

            // check if this node is the final node
            if (currentNode == endNode)
            {
                // reached the final node and return the calculated path
                return CalculatePath(endNode);
            }

            // remove the node from the open list and add it into the clostlist
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            // loop through all the neighbour nodes of the currentNode
            foreach (PathNode neighbourNode in GetNeighbourNodeList(currentNode))
            {
                // if this node is inside close list, skip
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }

                // if the node is not walkable, skip
                if (!neighbourNode.IsWalkable())
                {
                    // add to close list
                    closeList.Add(neighbourNode);
                    continue;
                }


                // else, calculate the g cost of this neighbourNode
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                // if the g cost of the tentativeG is smaller than its original G cost, the current result is a better way
                // the default initial gcost is int max
                if (tentativeGCost < neighbourNode.GetGCost())
                {

                    // remember its previouse valid node
                    neighbourNode.SetCameFromPathNode(currentNode);
                    // set the g cost of this neighbour node
                    neighbourNode.SetGCost(tentativeGCost);
                    // set the h cost of the neighbour node
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPos_));
                    // calculate the f cost
                    neighbourNode.CalculateFCost();

                    // add the neighbour node into the open list
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }


                }
            }


        }


        // no path has been found
        return null;

    }


    // function used to calcualte the distance from A to B, this is used to calculate the H cost
    private int CalculateDistance(GridPosition a_, GridPosition b_)
    {
        GridPosition gridPositionDifference = a_ - b_;
        // calculate the rough distance (width distance + length distance)
        int xDistance = Mathf.Abs(gridPositionDifference.x);
        int zDistance = Mathf.Abs(gridPositionDifference.z);

        // calculate the straight distance, which is the remaining difference of the two distance
        int straightDistance = Mathf.Abs(xDistance - zDistance);

        // return the distance multiply with the cost index
        return move_Diagonal_Cost * Mathf.Min(xDistance, zDistance) + move_Straight_Cost * straightDistance;
    }


    // function used to loop through all nodes inside a list and return the one with the lowest F cost
    private PathNode GetLowestFCostNodeFromList(List<PathNode> pathNodeList_)
    {
        PathNode LowestPathNode = pathNodeList_[0];
        // cycle the list
        for (int i = 0; i < pathNodeList_.Count; i++)
        {
            if (pathNodeList_[i].GetFCost() < LowestPathNode.GetFCost())
            {
                // replace the lowest
                LowestPathNode = pathNodeList_[i];
            }
        }

        // return the lowest
        return LowestPathNode;
    }

    // function used to get the path node at given x and z pos
    private PathNode GetPathNodeAtGridPosition(int xPos_, int zPos_)
    {
        // find the node
        return pfGridSystem.GetGridObject(new GridPosition(xPos_, zPos_));
    }


    // function used the get the neighbour nodes of the current node
    private List<PathNode> GetNeighbourNodeList(PathNode currentNode_)
    {
        // list to store all the neighoubours
        List<PathNode> NeighbourList = new List<PathNode>();

        // get the current node grid position
        GridPosition gridPosition = currentNode_.GetGridPosition();

        // firest check if the x and z pos is valid and inside the map
        if (gridPosition.x - 1 >= 0)
        {
            // find nearby nodes and add into the list
            // left
            NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x - 1, gridPosition.z + 0));

            // check the bottom bundary
            if (gridPosition.z - 1 >= 0)
            {
                // left Down
                NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x - 1, gridPosition.z - 1));

            }

            // check the top limit
            if (gridPosition.z + 1 < pfGridSystem.GetGridSystemHeight())
            {
                // left Up
                NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x - 1, gridPosition.z + 1));
            }
           
        }

        if (gridPosition.x + 1 < pfGridSystem.GetGridSystemWidth())
        {
            // right
            NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // right Down
                NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x + 1, gridPosition.z - 1));
            }
            
            if (gridPosition.z + 1 < pfGridSystem.GetGridSystemHeight())
            {
             
                // right Up
                NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x + 1, gridPosition.z + 1));
            }
            
        }


        // Up
        if (gridPosition.z + 1 < pfGridSystem.GetGridSystemHeight())
        {
            NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x + 0, gridPosition.z + 1));
        } 
        // Down
        if (gridPosition.z - 1 >= 0)
        {
            NeighbourList.Add(GetPathNodeAtGridPosition(gridPosition.x + 0, gridPosition.z - 1));
        }
            

        return NeighbourList;
    }


    // list used to return the node path list of the final path output
    private List<GridPosition> CalculatePath(PathNode endNode_)
    {
        // list constains all the selected nodes
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode_);


        // cycle through all the cameFromNodes and add them into the list
        PathNode currentNode = endNode_;
        while (currentNode.GetCameFromPathNode() != null)
        {
            // add it to the list and reset the currentnode
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        // reverse the path node list order so it start from the very first one
        pathNodeList.Reverse();

        // list to store all the accroding grid position
        List<GridPosition> gridPositionList = new List<GridPosition>();

        foreach(PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }


}
