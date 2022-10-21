using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    // the length of the map on x axis
    private int xlength;

    // the length of the map on z axis
    private int zlength;

    // the size of each individual floor cell
    private float cellSize;

    // 2d array used to store all the grid objects inside his gridsystem
    private GridObject[,] gridObjectArray;


    // constructor
    public GridSystem(int xlength_, int zlength_, float cellSize_)
    {
        // sign the with and height of the grid
        this.xlength = xlength_;
        this.zlength = zlength_;
        this.cellSize = cellSize_;

        // create an array with the size xlength and zlength
        gridObjectArray = new GridObject[xlength,zlength];

        // visual
        // populate all the grid objects on each grid center
        for (int x = 0; x< xlength_; x++)
        {
            for (int z = 0; z < zlength_; z++)
            {
                // create the grid positon for this grid
                GridPosition gridPosition = new GridPosition(x,z);
                // create a new grid object on this position and sign this gridobject into the array
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
                //Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x, z) + Vector3.right * 0.2f, Color.white, 1000);
            }
        }
        
    }


    // this function is for debug use
    // visulize each grid object by instantiate the debug prefab with the grid info on it
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x=0; x < xlength; x++)
        {
            for (int z = 0; z < zlength; z++)
            {
                // get the grid position
                GridPosition gridPosition = new GridPosition(x, z);

                // instantiate the debugprefab at the gridpostion
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                
                // find the script inside the debugPrefab
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                
                // sign the gridobject to the debugObject so it can visulize its grid positon on text 
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }



    // this function is used to return the gridobject from the array based on its gridPosition
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }



    // funciton used to transfer the x,z position of the grid into world position (vector 3)
    public Vector3 GetWorldPosition(GridPosition gridPosition_)
    {
        return new Vector3(gridPosition_.x,0, gridPosition_.z) * cellSize;
    }


    // function returns the grid position from a vector3
    public GridPosition GetGridPosition(Vector3 worldPosition_)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition_.x / cellSize), Mathf.RoundToInt(worldPosition_.z / cellSize));

    }

    // function used to test if one gridPosition is a valid postion (on the grid system)
    public bool IsValidGridPosition(GridPosition gridPosition_)
    {
        // compare the mini and max boundary
        return gridPosition_.x >= 0 && gridPosition_.z >= 0 && gridPosition_.x < xlength && gridPosition_.z < zlength;
    }

    // function used to return the width of the current grid system
    public int GetGridSystemWidth()
    {
        return xlength;
    }

    // function used to return the height of the current grid system
    public int GetGridSystemHeight()
    {
        return zlength;
    }



}
