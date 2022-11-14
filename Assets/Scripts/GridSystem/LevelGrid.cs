using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is used to manage the entire game grid system 
public class LevelGrid : MonoBehaviour
{
    // expose the class by make an static instance of the class
    public static LevelGrid Instance { get; private set; }

    // the prefab for the griddebugobject
    [SerializeField]
    private Transform gridDebugObjectPrefab;

    // refer to the grid system of the game
    private GridSystem<GridObject> gridSystem;

    // event fird on any unit grid pos change
    public event EventHandler OnAnyUnitGridPosChange;

    private void Awake()
    {
        // there should be only one instance of this class exist
        // check if there's multiple instance for this class, if so, destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }

        // if not instance yet, sign the instance
        Instance = this;

        // create a new grid system for the game
        gridSystem = new GridSystem<GridObject>(10, 10, 2.0f, 
            (GridSystem<GridObject> g_, GridPosition gridPosition_) => new GridObject(g_, gridPosition_));
        // populate the gridDebugObjects onto each grid
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }


    // this function sign the unit on the grid when its standing on it
    public void SetUnitAtGridPosition(GridPosition gridPosition_, UnitBasic unit_)
    {
        // locate the grid object on this position
        GridObject gridObject = gridSystem.GetGridObject(gridPosition_);
        // sign the unit to the gridObject
        gridObject.AddUnit(unit_);
    }

    // this function returns which unit is standing on the grid
    public List<UnitBasic> GetUnitListAtGridPosition(GridPosition gridPosition_)
    {
        // locate the grid object on this position
        GridObject gridObject = gridSystem.GetGridObject(gridPosition_);
        // return the signed unit
        return gridObject.GetUnitList();

    }


    // this function clear the unit at the grid position
    public void RemoveUnitAtGridPosition(GridPosition gridPosition_, UnitBasic unit_)
    {
        // locate the grid object on this position
        GridObject gridObject = gridSystem.GetGridObject(gridPosition_);
        // clear the unit to the gridObject
        gridObject.RemoveUnit(unit_);

    }

    // function used to return the gridpostion of an unit from its world position
    public GridPosition GetGridPosition(Vector3 worldPosition_)
    {
        return gridSystem.GetGridPosition(worldPosition_);
    }

    // function used to return the world postion of an unit from its grid position
    public Vector3 GetWorldPosition(GridPosition gridPosition_)
    {
        return gridSystem.GetWorldPosition(gridPosition_);
    }


    // this function is called when unit moved from one grid to another
    public void UpdateUnitGridPosition(UnitBasic unit_, GridPosition oldGridPos_, GridPosition newGridPos_)
    {
        // cleanup the old position
        RemoveUnitAtGridPosition(oldGridPos_, unit_);
        // setup the new positon
        SetUnitAtGridPosition(newGridPos_, unit_);
        // fire the unit grid pos change event
        OnAnyUnitGridPosChange?.Invoke(this, EventArgs.Empty);
    }

    // function used to expose the grid validation function from the gridSystem
    public bool IsValidGridPosition(GridPosition gridPosition_)
    {
        return gridSystem.IsValidGridPosition(gridPosition_);
    }

    // function used to check if one grid position is already occupied by other unit
    public bool IsGridPostionOccupied(GridPosition gridPosition_)
    {
        // find the grid object
        GridObject gridObject = gridSystem.GetGridObject(gridPosition_);
        // check if its unit list is not empty
        return gridObject.IsOccupied();

    }


    // function used to return the unit that occupying the grid
    public UnitBasic GetUnitOnGrid(GridPosition gridPosition_)
    {
        // find the grid object
        GridObject gridObject = gridSystem.GetGridObject(gridPosition_);
        // try get the unit on the grid position
        return gridObject.GetFirstUnitOnList();

    }




    // function used to expose the getwidth function in gridsystem
    public int GetSystemWidth()
    {
        return gridSystem.GetGridSystemWidth();
    }

    // function used to expose the getheight function in gridsystem
    public int GetSystemHeight()
    {
        return gridSystem.GetGridSystemHeight();
    }



}
